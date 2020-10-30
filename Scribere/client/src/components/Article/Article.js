import React, { useContext, useState } from "react";
import { ArticleContext } from "../../providers/ArticleProvider";
import { useHistory } from "react-router-dom";
import DangerButton from "../Buttons/DangerButton";
import PrimaryButton from "../Buttons/PrimaryButton";

import { TiDeleteOutline as DeleteArticleButton } from "react-icons/ti";
import { Modal, ModalHeader, ModalFooter } from "reactstrap";

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';


const useStyles = makeStyles({
    card: {
        maxWidth: 345,
    },
    media: {
        height: 140,
    },
});

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


    const classes = useStyles();

    const history = useHistory();

    const goDetails = () => {
        history.push(`/articles/${article.id}`);
    }





    return (



        <Card>
            <CardActionArea onClick={goDetails}>
                {(article.articleImage !== null) &&
                    <CardMedia
                        className={classes.media}
                        image={article.articleImage.imageUrl}
                        title={article.heading}
                    />}
                <CardContent>

                    <Typography gutterBottom variant="h5" component="h2">
                        {article.heading} </Typography>
                    <Typography gutterBottom variant="h5" component="h2">Category: {article.category.type}</Typography>
                    <Typography gutterBottom variant="h5" component="h3">
                        Nom de Plume: {article.userData.pseudonym}
                    </Typography>
                    {new Intl.DateTimeFormat('en-US').format(new Date(article.createDate))}



                </CardContent>
            </CardActionArea>
            <CardActions style={{ display: 'flex', justifyContent: 'space-between' }}>
                <PrimaryButton size="small" handleClick={goDetails}>Details</PrimaryButton>
                <DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={toggle}>Delete</DangerButton>
                {(article.userId === thisUser.id) &&

                    <div className="control__group">
                        {/* <button type="button" key={`DeleteRating${article.id}`} title="Delete" onClick={toggle}><DeleteArticleButton /></button> */}
                        <Modal isOpen={modal} toggle={toggle}>
                            <ModalHeader toggle={toggle}>Are you sure you want to delete this article?</ModalHeader>
                            <ModalFooter>
                                <Button color="secondary" onClick={handleDelete}>Delete</Button>{' '}
                                <Button color="primary" onClick={toggle}>Cancel</Button>
                            </ModalFooter>
                        </Modal>
                    </div>
                }
            </CardActions>

        </Card>

    )
}

export default Article;