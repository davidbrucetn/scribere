import React, { useContext, useState } from 'react';
import { CommentContext } from "../../providers/CommentProvider";
import { Button } from "reactstrap";
import "./Comment.css";



export default function CommentForm(article) {

    const thisUser = JSON.parse(sessionStorage.UserData)

    const [comment, setComment] = useState({ text: '', userId: thisUser.id, articleId: '', createDate: '' });

    const { addComment } = useContext(CommentContext);

    const handleFieldChange = evt => {
        const stateToChange = { ...comment };
        stateToChange[evt.target.id] = evt.target.value;
        setComment(stateToChange);
    };

    const goAddComment = () => {
        comment.createDate = new Date();
        comment.articleId = parseInt(article.id);

        addComment(comment)
    }

    const cancelNewComment = () => {

    }


    return (

        <>
            <div className="card" style={{ margin: "1em 1em", padding: "1em 1em" }}>
                <h6>Add a Comment</h6>
                <form id="form__comment__new">
                    <div className="form__comment__new">

                        <input className="form-control" id="text" value={comment.text} onChange={handleFieldChange} />
                    </div>
                </form>

                <Button color="primary" onClick={goAddComment}>Add Comment</Button>{" "}
                <Button onClick={cancelNewComment}>Cancel</Button>
            </div>
        </>
    )
}