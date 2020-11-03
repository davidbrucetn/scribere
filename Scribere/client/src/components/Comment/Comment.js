import React, { useContext, useState } from 'react';
import { CommentContext } from "../../providers/CommentProvider";
import { Button, Modal, ModalHeader, ModalFooter } from "reactstrap";
import "./Comment.css";

import { AiOutlineEdit as EditCommentButton } from "react-icons/ai";
import { TiDeleteOutline as DeleteCommentButton } from "react-icons/ti";


export default function Comment({ comment, article }) {
    const [editStatus, setEdit] = useState(false)
    const [editedComment, setComment] = useState();


    const { updateComment, deleteComment, GetAllCommentsByArticle } = useContext(CommentContext);
    const thisUser = JSON.parse(sessionStorage.UserData)
    const thisUserId = thisUser.id
    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);


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
                            <button type="button" key={`EditRating${comment.id}`} title="Edit" onClick={goEdit}><EditCommentButton /></button>
                        }
                        {((comment.userId === thisUserId) || (article.userId === thisUserId)) &&
                            <>
                                <button type="button" key={`DeleteRating${comment.id}`} title="Delete" onClick={toggle}><DeleteCommentButton /></button>

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
                                <button type="button" key={`DeleteRating${comment.id}`} title="Delete" onClick={toggle}><DeleteCommentButton /></button>

                                <Modal isOpen={modal} toggle={toggle}>
                                    <ModalHeader toggle={toggle}>Are you sure you want to delete?</ModalHeader>
                                    <ModalFooter>
                                        <Button color="danger" onClick={DeleteComment}>Delete</Button>{' '}
                                        <Button color="secondary" onClick={toggle}>Cancel</Button>
                                    </ModalFooter>
                                </Modal>
                            </>
                        }

                    </div>
                    <Button onClick={cancelEdit}>Cancel</Button>
                </div>
            </>
    )
}