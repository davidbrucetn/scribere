import React, { useEffect, useContext, useState } from "react";
import { useHistory, useParams } from 'react-router-dom';
import { ArticleContext } from "../../providers/ArticleProvider";
import { FavoriteContext } from "../../providers/FavoriteProvider";
import { ArticleTagContext } from "../../providers/ArticleTagProvider";
import { TagContext } from "../../providers/TagProvider";
import { CommentContext } from "../../providers/CommentProvider";
import "../Comment/Comment.css";
import Comment from "../Comment/Comment";
import DangerButton from "../Buttons/DangerButton";
import PrimaryButton from "../Buttons/PrimaryButton";
import EditCard from "../Cards/EditCard"

import { useForm } from "react-hook-form"
import * as Yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';

import "./ArticleDetail.css";


import { Dialog } from "@reach/dialog";


import Paper from "@material-ui/core/Paper";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import DialogActions from '@material-ui/core/DialogActions';
import CssBaseline from '@material-ui/core/CssBaseline';
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';
import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';



const ArticleDetail = () => {

    const [showDialog, setShowDialog] = React.useState(false);
    const [articleTags, setArticleTags] = useState([]);
    const open = () => setShowDialog(true);
    const close = () => setShowDialog(false);

    const [isLoading, setIsLoading] = useState(true);
    const [article, setArticle] = useState();
    const { getArticlebyId, deleteArticle } = useContext(ArticleContext);
    const { getTagsByArticleId, addArticleTag, deleteTagsByArticleId } = useContext(ArticleTagContext);
    const { tags, getAllTags } = useContext(TagContext);
    const { addFavoriteArticle, addFavoriteAuthor, removeFavoriteArticleId, removeFavoriteAuthorId, favoriteArticles, favoriteAuthors, getAllFavoriteArticleIds, getAllFavoriteAuthorIds } = useContext(FavoriteContext);
    const { comments, addComment, GetAllCommentsByArticle } = useContext(CommentContext);


    const { id } = useParams();
    const history = useHistory();
    const thisUser = JSON.parse(sessionStorage.UserData)

    const [comment, setComment] = useState({ text: '', userId: thisUser.id, articleId: '', createDate: '' });


    // Material-ui Styling

    const useStyles = makeStyles({
        card: {
            borderRadius: 2,
            backgroundColor: 'default',
            paddingBottom: 5,
            marginBottom: 3,
            marginTop: 3,
            paddingTop: 3,
            paddingLeft: '2em'
        },
        media: {
            height: '20vh',
            width: '20vw',
            maxWidth: '40%',
            borderRadius: '.3em',
            boxShadow: '10px 10px 15px #aaaaaa',
            paddingBottom: '2em',
        },
        Typography: {
            fontFamily: [
                'Merriweather',
                'serif'
            ].join(','),
            whiteSpace: 'pre-line'
        }
    });

    const handleFieldChange = evt => {
        const stateToChange = { ...comment };
        stateToChange[evt.target.id] = evt.target.value;
        setComment(stateToChange);
    };

    const classes = useStyles();
    // End Material-UI styling 


    const submitComment = (e) => {
        setIsLoading(true)
        comment.createDate = new Date();
        comment.articleId = article.id;
        addComment(comment)
            .then(() => {
                GetAllCommentsByArticle(article.id);
                setComment({ text: '', userId: thisUser.id, articleId: '', createDate: '' })

                setIsLoading(false)

            })
    };


    const cancelNewComment = () => {

    }

    const handleDelete = () => {
        setShowDialog(true);
    };

    const goEdit = () => {
        if (thisUser.id === article.userId) {
            history.push(`/articles/edit/${article.id}`);
        }

    }





    const Delete = () => {
        setShowDialog(false)
        deleteArticle(article.id)
            .then(() => {
                history.push("/articles");
            })
    }

    const AddFaveArticle = () => {
        const favoriteArticle = {
            "articleId": article.id
        }
        setIsLoading(true);
        addFavoriteArticle(favoriteArticle)
            .then(() => {
                GenerateDetail(id);
            })
    }

    const RemoveFaveArticle = () => {
        const favoriteArticle = {
            "articleId": article.id
        }
        setIsLoading(true);
        removeFavoriteArticleId(favoriteArticle.articleId)
            .then(() => {
                GenerateDetail(id);
            })
    }

    const AddFaveAuthor = () => {
        const favoriteAuthor = {
            "favoriteUserId": article.userId
        }
        setIsLoading(true);
        addFavoriteAuthor(favoriteAuthor)
            .then(() => {
                GenerateDetail(id);
            })
    }

    const RemoveFavAuthor = () => {

        const favoriteAuthor = {
            "favoriteUserId": article.userId
        }
        setIsLoading(true);
        removeFavoriteAuthorId(favoriteAuthor.favoriteUserId)
            .then(() => {
                GenerateDetail(id);
            })
    }

    const GenerateDetail = (id) => {

        getArticlebyId(id)
            .then((articleResp) => {
                setArticle(articleResp);
                getAllFavoriteAuthorIds();
                getAllFavoriteArticleIds();
                GetAllCommentsByArticle(id);

            })

        getTagsByArticleId(id)
            .then((articleTagResp) => {
                setArticleTags(articleTagResp)
                setIsLoading(false);
            });
    }

    // Tag Form
    let newTags = [];
    const schema = Yup.object().shape({
        tag_Ids: Yup.array()
            .transform(function (o, obj) {
                return o.filter(o => o);
            })
            .min(2, "")
    });

    const { register, handleSubmit, control, getValues, setValue } = useForm({
        resolver: yupResolver(schema),
        defaultValues: Object.fromEntries(
            tags.map((tag, i) => [
                `tag.id[${i}]`,
                articleTags.some(articleTag => articleTag.id === tags[i].id)
            ])
        )
    });

    const goBack = () => {
        history.push("/articles");
    }

    const onSubmit = (data, evt) => {
        Object.keys(data).forEach(key => {
            if (data[key] !== false) {
                newTags.push(
                    {
                        "articleId": parseInt(article.id),
                        "tagId": parseInt(data[key])
                    })
            };
        });
        setIsLoading(true)
        deleteTagsByArticleId(article.id)
            .then((p) => {
                if (!newTags.length > 0) {
                    getTagsByArticleId(article.id)
                        .then((articleTagResp) => {
                            setArticleTags(articleTagResp);
                            getAllTags();
                            setIsLoading(false);
                        })
                    history.push(`/articles/${article.id}`);

                }
                else {
                    // eslint-disable-next-line array-callback-return
                    newTags.map((articleTag) => {
                        addArticleTag(articleTag);
                    });
                    getTagsByArticleId(article.id)
                        .then((articleTagResp) => {
                            setArticleTags(articleTagResp);
                            getAllTags();
                            setIsLoading(false);
                        });

                }

            })
    };


    // End Tag Form


    useEffect(() => {
        GenerateDetail(id);
        getAllTags();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [id]);

    useEffect(() => {

    }, [articleTags])

    return (



        (!isLoading && article !== undefined) ?
            (
                <React.Fragment>
                    <CssBaseline />
                    <Container maxWidth="lg">
                        {<Dialog aria-label="deleteConfirm" className="dialog__fade"
                            isOpen={showDialog} onDismiss={close}
                            onClose={close}
                        >Are you sure you want to delete this article?
                             <DialogActions>

                                <PrimaryButton size="small" handleClick={close}>Disagree</PrimaryButton>
                                <DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={Delete}>Agree</DangerButton>

                            </DialogActions>
                        </Dialog>}
                        <Card className={classes.card} style={{ backgroundColor: '#f5f5f5' }}>
                            <CardActionArea>

                                {(article.articleImage !== "") ?
                                    <CardMedia
                                        className={classes.media}
                                        image={article.articleImage.imageUrl}
                                        title={article.heading}
                                    /> : null}
                                <EditCard handleClick={goEdit} >

                                    <Typography className={classes.Typography} gutterBottom variant="h5" component="h2">
                                        {article.heading} </Typography>
                                    <Typography className={classes.Typography} gutterBottom variant="h6" >Category: {article.category.type}</Typography>
                                    <Typography className={classes.Typography} gutterBottom variant="h6" >
                                        Author: {article.userData.pseudonym}
                                    </Typography>
                                    {new Intl.DateTimeFormat('en-US').format(new Date(article.createDate))}
                                    <br />

                                </EditCard>


                            </CardActionArea>
                            {(articleTags !== null || articleTags !== undefined) && articleTags.map((tag, index) =>
                                <div className="div__tag" key={`${article.id}-${tag.id}`} id={`${article.id}-${tag.id}`} style={{ display: 'inline-block', margin: '1vh .8vw', padding: '0 .4vw' }}>
                                    <h6 className="tag__title"> {tag.title}</h6>
                                </div>
                            )
                            }
                            <CardActions style={{ display: 'flex', justifyContent: 'space-between' }}>
                                {(article.userId === thisUser.id) && (
                                    <>
                                        <PrimaryButton size="small" handleClick={goEdit}>
                                            Edit
                                        </PrimaryButton>
                                        <DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={handleDelete}>Delete</DangerButton>
                                    </>)}
                                {(thisUser.id !== article.userId) &&
                                    ((favoriteAuthors.find((favAuthor) => { return favAuthor.favoriteUserId === article.userId })) ?

                                        <PrimaryButton className="btn__unfaveauthor bg-info" handleClick={RemoveFavAuthor}>UnFav Author</PrimaryButton>
                                        :
                                        <PrimaryButton className="btn__faveauthor bg-primary" handleClick={AddFaveAuthor}>Fav Author</PrimaryButton>)}
                                {(thisUser.id !== article.userId) &&
                                    ((favoriteArticles.find((favArticle) => { return favArticle.articleId === article.id })) ?

                                        <PrimaryButton className="btn__unfavearticle bg-info" handleClick={RemoveFaveArticle}>UnFavorite Article</PrimaryButton>
                                        :
                                        <PrimaryButton className="btn__favearticle bg-primary" handleClick={AddFaveArticle}>Fav Article</PrimaryButton>)}
                            </CardActions>
                            {(article.userId === thisUser.id) && (
                                <div className="wrap-collapsible">
                                    <input id="collapsible" className="toggle" type="checkbox" />
                                    <label htmlFor="collapsible" className="lbl-toggle">Manage Tags</label>
                                    <div className="div__collapsible__tag--form">
                                        <div className="div__tag__form">
                                            <Paper
                                                component="form"
                                                onSubmit={handleSubmit(onSubmit)}
                                            >
                                                <div className="div__tags__list">
                                                    {tags.map((tag, i) => {
                                                        return (
                                                            <FormCheckBox
                                                                key={tag.id}
                                                                name={tag.title}
                                                                control={control}
                                                                setValue={setValue}
                                                                getValues={getValues}
                                                                value={tag.id}
                                                                label={tag.title}
                                                                register={register}
                                                                defaultValue={articleTags.some(articleTag => articleTag.id === tag.id)}
                                                            />
                                                        );
                                                    })}
                                                </div>

                                                <Button
                                                    variant="contained"
                                                    color="primary"
                                                    size="large"
                                                    type="submit"
                                                > Save </Button>
                                                <Button
                                                    variant="contained"
                                                    color="secondary"
                                                    size="large"
                                                    onClick={goBack}

                                                > Cancel</Button>
                                            </Paper>

                                        </div>
                                    </div>
                                </div>
                            )}

                            <div className="div__comment__form">
                                <div className="card" style={{ margin: "1em 1em", padding: "1em 1em" }}>
                                    <h6>Add a Comment</h6>
                                    <form id="form__comment__new">
                                        <div className="form__comment__new">

                                            <input className="form-control" id="text" value={comment.text} onChange={handleFieldChange} />
                                        </div>
                                    </form>

                                    <Button color="primary" onClick={submitComment}>Add Comment</Button>{" "}
                                    <Button onClick={cancelNewComment}>Cancel</Button>
                                </div>

                            </div>

                        </Card>
                        <Typography className={classes.Typography} component="div" style={{ color: '#212529', backgroundColor: '#fff', height: 'auto', padding: '1em' }} >

                            {article.text}
                        </Typography>
                        <Container>

                            {comments.map((comment) => <Comment key={`${article.id} - ${comment.id}`} comment={comment} article={article} />)}
                        </Container>
                    </Container>
                </React.Fragment >

            ) : null

    )


}

export const FormCheckBox = ({
    name,
    value,
    register,
    control,
    setValue,
    getValues,
    defaultValue
}) => {
    return (
        <FormControlLabel
            control={<Checkbox defaultChecked={defaultValue} />}
            name={name}
            inputRef={register}
            value={value}
            label={name}
        />
    );
};

export default ArticleDetail;