import React, { useContext, useEffect, useState } from "react";
import { useHistory, useParams } from 'react-router-dom';

import { UserDataContext } from "../../providers/UserDataProvider";
import { UserBlockContext } from "../../providers/UserBlockProvider";
import { UserBlockMgr } from "../Helper/DWBUtils";

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

// let thisUserData = {};

const UserDetail = () => {

    const [isLoading, setIsLoading] = useState(true);
    const [userData, setUserData] = useState({});
    const [thisUserData, setThisUserData] = useState(JSON.parse(sessionStorage.UserData))
    const { deleteUser, getAllUsers, getUserById } = useContext(UserDataContext);
    const { userBlocks, addUserBlock, deleteUserBlock, getAllUserBlocks } = useContext(UserBlockContext);

    // thisUserData = JSON.parse(sessionStorage.UserData);

    const { id } = useParams();



    const generateUserDetail = (id) => {
        getAllUserBlocks();
        getUserById(id)
            .then(setUserData)
        setIsLoading(false);

    }






    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);

    const handleDelete = () => {
        toggle();
        deleteUser(userData.id)
        getAllUsers();

    }

    const goEdit = () => {
        if (thisUserData.id === userData.id) {
            history.push(`/users/edit/${userData.id}`);
        }

    }

    const goBlock = () => {
        const userBlock = {
            "sourceUserId": thisUserData.id,
            "blockedUserId": userData.id
        };
        addUserBlock(userBlock)
            .then(() => {
                generateUserDetail(id);
            })
    }

    const goUnBlock = () => {
        deleteUserBlock(userData.id)
            .then(() => {
                generateUserDetail(id);

            });
    }


    const classes = useStyles();

    const history = useHistory();

    const goDetails = () => {
        history.push(`/users/${userData.id}`);
    }

    useEffect(() => {
        generateUserDetail(id);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [id])

    return (

        (!isLoading) ?

            <Card>
                <CardActionArea onClick={goDetails}>

                    <CardContent>
                        <Typography gutterBottom variant="h5" component="h2">{userData.nameFirst} {userData.nameLast} </Typography>
                        <Typography gutterBottom variant="h5" component="h3">Nom de Plume: {userData.pseudonym}</Typography>
                        <Typography gutterBottom variant="h6" component="h6">City: {userData.city}</Typography>
                        <Typography gutterBottom variant="h6" component="h6">State: {userData.state}</Typography>
                    </CardContent>
                </CardActionArea>

                <CardActions style={{ display: 'flex', justifyContent: 'space-between' }}>
                    {(userData.id !== thisUserData.id) && (
                        (!UserBlockMgr(userData.id, thisUserData.id, userBlocks)) ?
                            <DangerButton DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={goBlock}>Block User</DangerButton> : <DangerButton DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={goUnBlock}>UnBlock User</DangerButton>
                    )
                    }
                    {(userData.id === thisUserData.id) &&
                        <>
                            <PrimaryButton size="small" handleClick={goEdit}>Edit</PrimaryButton>
                            <DangerButton ml="3em" size="small" className={classes.ButtonDanger} handleClick={toggle}>Delete</DangerButton>


                            <div className="control__group">
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

            </Card > : null

    )
}

export default UserDetail;