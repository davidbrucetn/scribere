import React, { useContext, useState } from "react";
import { ArticleContext } from "../../providers/ArticleProvider";
import { useHistory } from "react-router-dom";
import { Spinner } from "reactstrap";
import DangerButton from "../Buttons/DangerButton";
import PrimaryButton from "../Buttons/PrimaryButton";
import "./Article.css";

import { Modal, ModalHeader, ModalFooter } from "reactstrap";

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';
import { grey } from "@material-ui/core/colors";


const useStyles = makeStyles({
    card: {
        maxWidth: '75vw',
        border: '1px solid grey[200]',
        opacity: 1,
        animationName: 'fadeInOpacity',
        animationTimingFunction: 'ease-in',
        animationDuration: '1s',
        boxShadow: '10px 10px 15px #aaaaaa',
        margin: 15,

    },
    CardActionArea: {
        backgroundColor: grey[200],
    },
    media: {

        height: '40%',
        width: '40%',
        maxWidth: 400,
        maxHeight: 400,
        backgroundColor: "rgb(46, 79, 105)",
        boxShadow: '10px 10px 15px #aaaaaa'
    },
    TypographyShadow: {
        fontFamily: [
            'Merriweather',
            'serif'
        ].join(','),
        fontWeight: 100,
        textShadow: '10px 10px 15px #aaaaaa',
        whiteSpace: 'pre-line',
    }
});
let articleImageHolder = "";
const Article = ({ article, generateArticleList }) => {
    const { deleteArticle } = useContext(ArticleContext);


    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);

    const thisUser = JSON.parse(sessionStorage.UserData)

    const handleDelete = () => {
        toggle();
        deleteArticle(article.id)
        generateArticleList();

    }

    const goUserDetail = () => {
        history.push(`/users/${article.userId}`)
    }

    const classes = useStyles();

    const history = useHistory();

    const goDetails = () => {
        history.push(`/articles/${article.id}`);
    }

    if (article.articleImage.imageUrl === "") {
        articleImageHolder = './images/quillImage.jpg';
    } else {
        articleImageHolder = article.articleImage.imageUrl
    }

    return (
        <Card className={classes.card}>

            <CardActionArea className={classes.CardActionArea} onClick={goDetails}>
                <div className="card__media__container">
                    {(articleImageHolder === article.articleImage.imageUrl) ?
                        <CardMedia
                            component="img"
                            className={classes.media}
                            image={articleImageHolder}
                            title={article.heading} >
                        </CardMedia> :
                        <img src={require("./images/quillImage.jpg").default} alt={article.heading} />}
                    <div className="div__card__media--right">

                    </div>
                </div>
                <CardContent>

                    <Typography className={classes.TypographyShadow} gutterBottom variant="h4" component="h4">
                        {article.heading} </Typography>
                    <Typography className={classes.TypographyShadow} gutterBottom variant="h6" component="h6">
                        <table>
                            <tbody>
                                <tr>
                                    <th>
                                        <strong>Category</strong>
                                    </th>
                                    <td>
                                        {article.category.type}
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <strong>Author</strong>
                                    </th>
                                    <td>
                                        {article.userData.pseudonym}
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </Typography>
                    <span >{new Intl.DateTimeFormat('en-US').format(new Date(article.createDate))}</span>

                </CardContent>
            </CardActionArea>
            <CardActions style={{ display: 'flex', justifyContent: 'space-between' }}>
                <PrimaryButton size="small" handleClick={goDetails}>Details</PrimaryButton>
                <PrimaryButton size="small" handleClick={goUserDetail}>Author Details</PrimaryButton>
                {(article.userId === thisUser.id) &&
                    <>
                        <DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={toggle}>Delete</DangerButton>
                        <div className="control__group">
                            <Modal isOpen={modal} toggle={toggle}>
                                <ModalHeader toggle={toggle}>Are you sure you want to delete this article?</ModalHeader>
                                <ModalFooter>
                                    <Button color="secondary" onClick={handleDelete}>Delete</Button>{' '}
                                    <Button color="primary" onClick={toggle}>Cancel</Button>
                                </ModalFooter>
                            </Modal>
                        </div>
                    </>
                }
            </CardActions>

        </Card>

    )
}

export default Article;