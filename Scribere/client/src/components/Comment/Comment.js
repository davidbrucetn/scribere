import React, { useContext, useState } from 'react';
import { CommentContext } from "../../providers/CommentProvider";
import { Button, Modal, ModalHeader, ModalFooter } from "reactstrap";
import { useHistory } from 'react-router-dom';
import "./Comment.css";

import { AiOutlineEdit as EditCommentButton, AiOutlineSave } from "react-icons/ai";
import { TiDeleteOutline as DeleteCommentButton } from "react-icons/ti";


export default function Comment({ comment }) {
    const [editStatus, setEdit] = useState(false)
    const [editedComment, setComment] = useState();


    const { comments, addComment, getCommentById, GetAllCommentsByArticle, updateComment, deleteComment } = useContext(CommentContext);
    const thisUser = JSON.parse(sessionStorage.UserData)
    const thisUserId = thisUser.id
    const history = useHistory();
    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);


    const handleFieldChange = evt => {
        const stateToChange = { ...editedComment };
        console.log(evt.target)
        stateToChange[evt.target.id] = evt.target.value;
        setComment(stateToChange);
    };

    const Update = () => {
        comment.text = editedComment.text;
        updateComment(comment)
            .then(setEdit(false));
    }

    const DeleteComment = () => {
        deleteComment(comment.id);
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
                <div className="card" style={{ margin: "1em 1em" }}>
                    <p>{comment.text}</p>
                    <span>{comment.userData.pseudonym}</span>
                    <p className="text-secondary">{new Intl.DateTimeFormat('en-US').format(new Date(comment.createDate))}</p>
                    {(comment.userId === thisUserId) &&

                        <div className="control__group">
                            <button type="button" key={`DeleteRating${comment.id}`} title="Delete" onClick={toggle}><DeleteCommentButton /></button>
                            <button type="button" key={`EditRating${comment.id}`} title="Edit" onClick={goEdit}><EditCommentButton /></button>
                            <Modal isOpen={modal} toggle={toggle}>
                                <ModalHeader toggle={toggle}>Are you sure you want to delete?</ModalHeader>
                                <ModalFooter>
                                    <Button color="danger" onClick={DeleteComment}>Delete</Button>{' '}
                                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                                </ModalFooter>
                            </Modal>
                        </div>
                    }
                </div>
            </> :
            <>
                <div className="card" style={{ margin: "1em 1em" }}>
                    <h6>Edit Form</h6>
                    <label htmlFor="content" className="control-label">Content</label>
                    <input className="form-control" id="text" value={editedComment.text} onChange={handleFieldChange} />
                    <span>{comment.userData.pseudonym}</span>
                    <p className="text-secondary">{new Intl.DateTimeFormat('en-US').format(new Date(comment.createDate))}</p>
                    {(comment.userId === thisUserId) &&

                        <div className="control__group">
                            <Button color="primary" onClick={Update}>Update Comment</Button>{" "}
                            <button type="button" key={`DeleteRating${comment.id}`} title="Delete" onClick={toggle}><DeleteCommentButton /></button>
                            <button type="button" key={`EditRating${comment.id}`} title="Edit" onClick={goEdit}><EditCommentButton /></button>
                            <Modal isOpen={modal} toggle={toggle}>
                                <ModalHeader toggle={toggle}>Are you sure you want to delete?</ModalHeader>
                                <ModalFooter>
                                    <Button color="danger" onClick={DeleteComment}>Delete</Button>{' '}
                                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                                </ModalFooter>
                            </Modal>
                        </div>
                    }
                    <Button onClick={cancelEdit}>Cancel</Button>
                </div>
            </>
    )
}