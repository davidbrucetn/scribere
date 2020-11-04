import React, { useState, useContext, useEffect } from "react";
import { ArticleContext } from "../../providers/ArticleProvider";
import { CategoryContext } from "../../providers/CategoryProvider";
import { VisibilityContext } from "../../providers/VisibilityProvider";
import { useHistory } from "react-router-dom";


import "./ArticleForm.css";

import { makeStyles } from '@material-ui/core/styles';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';

const ArticleForm = () => {
    const { addArticle } = useContext(ArticleContext);
    const [article, setArticle] = useState({ heading: "", text: "", createDate: "", categoryId: 1, userId: 0, visibilityId: 2 })
    const [articleImage, setArticleImage] = useState({ imageUrl: "" })
    const { category, categories, getAllCategories } = useContext(CategoryContext);
    const { visibilities, getAllVisibilities } = useContext(VisibilityContext)
    const [visibilityValue, setVisibilityValue] = React.useState(2);
    const [isLoading, setIsLoading] = useState(false);

    const useStyles = makeStyles({

        formlabel: {
            borderRadius: 2,
            backgroundColor: 'default',
            paddingBottom: 5,
            marginBottom: 3,
            marginTop: 3,
            paddingTop: 3,
            paddingLeft: '2em',
        },
        Typography: {
            fontFamily: [
                'Merriweather',
                'serif'
            ].join(','),
            textShadow: '10px 10px 15px #aaaaaa',
            whiteSpace: 'pre-line',
        }
    });

    const classes = useStyles();




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
        article.createDate = new Date();
        article.userId = thisUser.id;
        article.categoryId = parseInt(article.categoryId);
        if (article.articleImage !== undefined) {
            article.articleImage.articleId = article.id;
        } else {
            article.articleImage = {
                "ImageUrl": ""
            };
        }

        addArticle(article).then((a) => {
            history.push(`/articles`);
        });
    };

    const cancel = () => {
        history.push(`/articles`);
    }


    return (
        <div className="container__article__new">
            <div className="row justify-content-center div__articleNew__form">
                <div className="card col-md-11 col-lg-10">
                    <h3 className="mt-3 text-center card-title main__title">Let the Words Flow</h3>
                    <div className="mt-2 card-body">
                        <div className="form-group">
                            <FormLabel className={classes.Typography} component="legend">Heading</FormLabel>
                            <input
                                id="heading"
                                className="form-control"
                                onChange={handleFieldChange}
                                value={article.heading}
                            />
                        </div>
                        <div className="form-group">
                            <FormLabel className={classes.Typography} component="legend">Article Image URL</FormLabel>
                            <input
                                id="articleImage.imageUrl"
                                className="form-control"
                                onChange={handleFieldChange}
                                value={articleImage.imageUrl}
                            />
                        </div>
                        <div className="form-group">
                            <FormLabel className={classes.Typography} component="legend">Category</FormLabel>
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
                        <div className="form-group radio__group">
                            <FormControl className={classes.Typography} component="fieldset">
                                <FormLabel className={classes.Typography} component="legend">Visibility</FormLabel>
                                <RadioGroup row aria-label="visibilityId" name="article.visibiltyId" value={visibilityValue} onChange={handleFieldChange} >
                                    {visibilities.map(aVisibility =>
                                        <FormControlLabel className={classes.Typography} key={aVisibility.id.toString() + "Visibility"} value={aVisibility.id.toString()} control={<Radio />} label={aVisibility.type} />
                                    )}
                                </RadioGroup>
                            </FormControl>
                        </div>
                        <div className="form-group">
                            <FormLabel className={classes.Typography} component="legend">Body</FormLabel>
                            <textarea
                                id="text"
                                rows="13"
                                className="form-control article__content-input"
                                onChange={handleFieldChange}
                                value={article.text}
                            >
                            </textarea>
                        </div>
                        <div className="form-group">
                            <button disabled={isLoading} onClick={submit} className="btn btn-primary btn-block btn-styling">Save</button>
                            <button disabled={isLoading} onClick={cancel} className="btn btn-primary btn-block btn-styling" style={{ backgroundColor: "gray" }}>Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
export default ArticleForm