import React, { useContext, useState } from 'react';
import { CommentContext } from "../../providers/CommentProvider";
import { Button, Modal, ModalHeader, ModalFooter } from "reactstrap";
import "./Comment.css";

import { AiOutlineEdit as EditCommentButton } from "react-icons/ai";
import { TiDeleteOutline as DeleteCommentButton } from "react-icons/ti";
import { makeStyles } from '@material-ui/core/styles';
import { blue } from '@material-ui/core/colors';


export default function Comment({ comment, article }) {
    const [editStatus, setEdit] = useState(false)
    const [editedComment, setComment] = useState();


    const { updateComment, deleteComment, GetAllCommentsByArticle } = useContext(CommentContext);
    const thisUser = JSON.parse(sessionStorage.UserData)
    const thisUserId = thisUser.id
    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);


    const useStyles = makeStyles((theme) => ({
        root: {
            display: 'flex',
            flexWrap: 'wrap',
            justifyContent: 'space-around',
            overflow: 'Hidden',
            backgroundColor: theme.palette.background.paper,
        },
        gridList: {
            width: '80vw',
            height: '90vh',
            opacity: 1,
            animationName: 'fadeInOpacity',
            animationTimingFunction: 'ease',
            animationDuration: '2s',
            paddingTop: 10,

        },
        icon: {
            color: 'rgba(255, 255, 255, 0.54)',
        },
        buttonSave: {
            backgroundColor: '#0063cc',
        },
        buttonDelete: {
            backgroundColor: '#d50000',
            size: 'small',
        },
        Typography: {
            fontFamily: [
                'Merriweather',
                'serif'
            ].join(','),
            whiteSpace: 'pre-line'
        },
        card: {
            margin: '0 1em',
        }
    }));

    const classes = useStyles();

    const handleFieldChange = evt => {
        const stateToChange = { ...editedComment };
        stateToChange[evt.target.id] = evt.target.value;
        setComment(stateToChange);
    };

    const Update = () => {
        comment.text = editedComment.text;
        updateComment(comment)
            .then(setEdit(false));
    }

    const DeleteComment = () => {
        deleteComment(comment.id)
            .then(toggle)
            .then(GetAllCommentsByArticle(comment.articleId))
    }

    const goEdit = () => {
        setComment(comment)
        setEdit(true);
    }

    const cancelEdit = () => {
        setEdit(false);
    }

    useState(() => {

    }, [editStatus])


    return (
        (!editStatus) ?
            <>
                <div className="card card__comment" style={{ margin: "1em 1em" }}>
                    <p>{comment.text}</p>
                    <span>{comment.userData.pseudonym}</span>
                    <p className="text-secondary">{new Intl.DateTimeFormat('en-US').format(new Date(comment.createDate))}</p>
                    <div className="control__group">
                        {(comment.userId === thisUserId) &&
                            <Button className={classes.buttonSave} key={`EditRating${comment.id}`} title="Edit" onClick={goEdit}>Edit</Button>
                        }
                        {((comment.userId === thisUserId) || (article.userId === thisUserId)) &&
                            <>
                                <Button className={classes.buttonDelete} key={`DeleteRating${comment.id}`} title="Delete" onClick={toggle}>Delete</Button>

                                <Modal isOpen={modal} toggle={toggle}>
                                    <ModalHeader toggle={toggle}>Are you sure you want to delete this comment?</ModalHeader>
                                    <ModalFooter>
                                        <Button color="danger" onClick={DeleteComment}>Delete</Button>{' '}
                                        <Button color="secondary" onClick={toggle}>Cancel</Button>
                                    </ModalFooter>
                                </Modal>
                            </>
                        }
                    </div>
                </div>
            </> :
            <>
                <div className="card card__comment" style={{ margin: "1em 1em" }}>
                    <h6>Edit Form</h6>
                    <label htmlFor="content" className="control-label">Content</label>
                    <textarea rows="2" className="form-control" id="text" value={editedComment.text} onChange={handleFieldChange} />
                    <span>{comment.userData.pseudonym}</span>
                    <p className="text-secondary">{new Intl.DateTimeFormat('en-US').format(new Date(comment.createDate))}</p>
                    <div className="control__group">
                        {(comment.userId === thisUserId) &&

                            <>
                                <Button color="danger" onClick={DeleteComment}>Delete</Button>{' '}


                                <Modal isOpen={modal} toggle={toggle}>
                                    <ModalHeader toggle={toggle}>Are you sure you want to delete?</ModalHeader>
                                    <ModalFooter>
                                        <Button color="danger" onClick={DeleteComment}>Delete</Button>{' '}
                                        <Button color="secondary" onClick={toggle}>Cancel</Button>
                                    </ModalFooter>
                                </Modal>
                            </>
                        }
                        <Button className={classes.buttonSave} onClick={Update}>Save</Button>
                        <Button onClick={cancelEdit}>Cancel</Button>
                    </div>

                </div>
            </>
    )
}