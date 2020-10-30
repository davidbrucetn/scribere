import React, { useContext, useState } from "react";
import { UserDataContext } from "../../providers/UserDataProvider";
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

const User = ({ userData, getAllUsers }) => {
    const { deleteUser } = useContext(UserDataContext);


    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);

    const thisUser = JSON.parse(sessionStorage.UserData)

    const handleDelete = () => {
        toggle();
        deleteUser(userData.id)
        getAllUsers();

    }


    const classes = useStyles();

    const history = useHistory();

    const goDetails = () => {
        history.push(`/users/${userData.id}`);
    }





    return (



        <Card>
            <CardActionArea onClick={goDetails}>

                <CardContent>
                    <Typography gutterBottom variant="h5" component="h2">{userData.nameFirst} {userData.nameLast}</Typography>
                    <Typography gutterBottom variant="h5" component="h3">Nom de Plume: {userData.pseudonym}</Typography>
                    <Typography gutterBottom variant="h6" component="h6">City: {userData.city}</Typography>
                    <Typography gutterBottom variant="h6" component="h6">State: {userData.state}</Typography>
                    <Typography gutterBottom variant="h6" component="h6">Country: {userData.country.name}</Typography>
                </CardContent>
            </CardActionArea>
            <CardActions style={{ display: 'flex', justifyContent: 'space-between' }}>
                <PrimaryButton size="small" handleClick={goDetails}>Details</PrimaryButton>
                {(userData.id === thisUser.id) &&
                    <>
                        <DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={toggle}>Delete</DangerButton>


                        <div className="control__group">
                            <button type="button" key={`DeleteRating${userData.id}`} title="Delete" onClick={toggle}><DeleteArticleButton /></button>
                            <Modal isOpen={modal} toggle={toggle}>
                                <ModalHeader toggle={toggle}>Are you sure you want to delete your account?</ModalHeader>
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

export default User;