import React, { useState, useContext, useEffect } from "react";
import { ArticleContext } from "../../providers/ArticleProvider";
import { CategoryContext } from "../../providers/CategoryProvider";
import { VisibilityContext } from "../../providers/VisibilityProvider";
import { useHistory } from "react-router-dom";

import "./ArticleForm.css";

import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';

const ArticleForm = () => {
    const { addArticle } = useContext(ArticleContext);
    const [article, setArticle] = useState({ heading: "", text: "", createDate: "", categoryId: 1, userId: 0, visibilityId: 2 })
    const { category, getAllCategories } = useContext(CategoryContext);
    const { visibilities, getAllVisibilities } = useContext(VisibilityContext)
    const [visibilityValue, setVisibilityValue] = React.useState(2);
    const [isLoading, setIsLoading] = useState(false);


    const history = useHistory();
    const thisUser = JSON.parse(sessionStorage.UserData);

    useEffect(() => {
        getAllCategories()
            .then((resp) => {
                getAllVisibilities();
                setVisibilityValue("2");
            })
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);



    const handleFieldChange = evt => {
        const stateToChange = { ...article }
        if (evt.target.type === "radio") {
            setVisibilityValue(evt.target.value);
            stateToChange["visibilityId"] = parseInt(evt.target.value);
        } else if (evt.target.id === "categoryId") {
            stateToChange[evt.target.id] = parseInt(evt.target.value)
        } else {
            stateToChange[evt.target.id] = evt.target.value
        }
        setArticle(stateToChange)
    }

    const submit = (e) => {
        setIsLoading(true)
        article.createDate = new Date();
        article.userId = thisUser.id;
        article.categoryId = parseInt(article.categoryId);
        article.articleImage = {
            "ImageUrl": ""
        };

        addArticle(article).then((a) => {
            // history.push(`/articles/${a.id}`);
            history.push(`/articles`);
        });


    };


    return (
        <div className="container__article__new">
            <div className="row justify-content-center div__articleNew__form">
                <div className="card col-md-11 col-lg-8">
                    <h3 className="mt-3 text-primary text-center card-title">Let the Words Flow</h3>
                    <div className="mt-2 card-body">
                        <div className="form-group">
                            <FormLabel component="legend">Heading</FormLabel>
                            <input
                                id="heading"
                                className="form-control"
                                onChange={handleFieldChange}
                                value={article.heading}
                            />
                        </div>
                        <div className="form-group">
                            <FormLabel component="legend">Category</FormLabel>
                            <select
                                id="categoryId"
                                className="form-control"
                                onChange={handleFieldChange}
                                value={article.categoryId}
                            >

                                {category.map(aCategory =>
                                    <option value={aCategory.id} key={aCategory.id}>{aCategory.type}</option>
                                )}
                            </select>
                        </div>
                        <div className="form-group radio__group">
                            <FormControl component="fieldset">
                                <FormLabel component="legend">Visibility</FormLabel>
                                <RadioGroup row aria-label="visibilityId" name="article.visibiltyId" value={visibilityValue} onChange={handleFieldChange} >
                                    {visibilities.map(aVisibility =>
                                        <FormControlLabel key={aVisibility.id.toString() + "Visibility"} value={aVisibility.id.toString()} control={<Radio />} label={aVisibility.type} />
                                    )}
                                </RadioGroup>
                            </FormControl>
                        </div>
                        <div className="form-group">
                            <label htmlFor="text" className="control-label">Body</label>
                            <textarea
                                id="text"
                                rows="15"
                                className="form-control article__content-input"
                                onChange={handleFieldChange}
                                value={article.text}
                            >
                            </textarea>
                        </div>
                        <div className="form-group">
                            <button disabled={isLoading} onClick={submit} className="btn btn-primary btn-block">Save</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
export default ArticleForm