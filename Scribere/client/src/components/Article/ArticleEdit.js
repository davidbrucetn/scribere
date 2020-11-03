import React, { useState, useContext, useEffect } from "react";
import { useHistory, useParams } from "react-router-dom";
import { ArticleContext } from "../../providers/ArticleProvider";
import { CategoryContext } from "../../providers/CategoryProvider";
import { VisibilityContext } from "../../providers/VisibilityProvider";


import "./ArticleForm.css";

import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';

const ArticleEdit = () => {
    const { updateArticle, getArticlebyId } = useContext(ArticleContext);
    const [article, setArticle] = useState({ heading: "", text: "", createDate: "", categoryId: 0, userId: 0 })
    const [articleImage, setArticleImage] = useState({ articleId: "", imageUrl: "" })
    const { category, categories, getAllCategories } = useContext(CategoryContext);
    const { visibilities, getAllVisibilities } = useContext(VisibilityContext)
    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(true);

    const history = useHistory();


    useEffect(() => {
        getAllCategories();
        getAllVisibilities();
        getArticlebyId(id)
            .then((articleResp) => {
                if (articleResp.articleImage === null) {

                    setArticleImage({
                        articleId: article.id,
                        imageUrl: ""
                    });
                    setArticle(article.articleImage = articleImage);
                    setArticle(articleResp);
                    setIsLoading(false);
                } else {
                    setArticleImage({
                        id: articleResp.articleImage.id,
                        articleId: id,
                        imageUrl: articleResp.articleImage.imageUrl
                    })
                    setArticle(articleResp);
                    setIsLoading(false);
                }

            });

        //eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);


    const handleFieldChange = evt => {
        const stateToChange = { ...article }
        if (evt.target.type === "radio") {
            stateToChange["visibilityId"] = parseInt(evt.target.value);
        } else if (evt.target.id === "articleImage.imageUrl") {
            stateToChange[evt.target.id] = evt.target.value;
            setArticleImage(article.articleImage = { "imageUrl": evt.target.value })
            setArticle(article)
        } else {
            stateToChange[evt.target.id] = evt.target.value
        }
        (evt.target.id !== "articleImage.imageUrl") &&
            setArticle(stateToChange)
    }

    const submit = (e) => {
        setIsLoading(true)
        article.categoryId = parseInt(article.categoryId);
        if (article.articleImage["imageUrl"] !== "") {
            article.articleImage.articleId = article.id;
        } else {
            article.articleImage = {
                "ImageUrl": ""
            };
        }
        updateArticle(article).then((a) => {
            history.push(`/articles/${article.id}`);
        });
    };

    const cancel = () => {
        history.push(`/articles`);
    }

    return (
        (!isLoading && article !== undefined) ?

            <div className="container">
                <div className="row justify-content-center">
                    <div className="card col-md-12 col-lg-15">
                        <h3 className="mt-3 text-primary text-center card-title">Let the Words Flow</h3>
                        <div className="mt-5 card-body">
                            <div className="text-danger"></div>
                            <div className="form-group">
                                <label htmlFor="heading" className="control-label">Heading</label>
                                <input
                                    id="heading"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={article.heading}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="articleImage.imageUrl" className="control-label">Article Image URL</label>
                                <input
                                    id="articleImage.imageUrl"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={articleImage.imageUrl}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="categoryId" className="control-label">Category</label>
                                <select
                                    id="categoryId"
                                    className="form-control"
                                    onChange={handleFieldChange}
                                    value={article.categoryId}
                                >
                                    {categories.map(aCategory =>
                                        <option value={aCategory.id} key={aCategory.id}>{aCategory.type}</option>
                                    )}
                                </select>
                            </div>
                            {(article.visibilityId !== undefined) &&
                                <div className="form-group radio__group">
                                    <FormControl component="fieldset">
                                        <FormLabel component="legend">Visibility</FormLabel>
                                        <RadioGroup row aria-label="visibilityId" name="article.visibiltyId" value={article.visibilityId.toString()} onChange={handleFieldChange} >
                                            {visibilities.map(aVisibility =>
                                                <FormControlLabel key={aVisibility.id.toString() + "Visibility"} value={aVisibility.id.toString()} control={<Radio />} label={aVisibility.type} />
                                            )}
                                        </RadioGroup>
                                    </FormControl>
                                </div>}
                            <div className="form-group">
                                <label htmlFor="text" className="control-label">Body</label>
                                <textarea
                                    id="text"
                                    rows="13"
                                    className="form-control post__content-input"
                                    onChange={handleFieldChange}
                                    value={article.text}
                                >
                                </textarea>
                            </div>
                            <div className="form-group">
                                <button disabled={isLoading} onClick={submit} className="btn btn-primary btn-block">SAVE</button>
                                <button disabled={isLoading} onClick={cancel} className="btn btn-primary btn-block">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div> : null
    )
}
export default ArticleEdit