import React, { useContext, useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { FilterforBlockedUsers } from "../Helper/DWBUtils";
import { UserDataContext } from "../../providers/UserDataProvider";
import { UserBlockContext } from "../../providers/UserBlockProvider";
import User from "./User";

import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import GridListTileBar from '@material-ui/core/GridListTileBar';
import ListSubheader from '@material-ui/core/ListSubheader';

let thisUserData = {};
const UserDataList = () => {

    const { users, getAllUsers, getUserById } = useContext(UserDataContext);
    const { userBlocks, addUserBlock, getAllUserBlocks, deleteUserBlock } = useContext(UserBlockContext);
    const [IsLoading, setIsLoading] = useState(true)
    thisUserData = JSON.parse(sessionStorage.UserData);


    const generateUserDataList = () => {
        Promise.all([getAllUsers(), getAllUserBlocks()])
        setIsLoading(false);
    }

    const useStyles = makeStyles((theme) => ({
        root: {
            display: 'flex',
            flexWrap: 'wrap',
            justifyContent: 'space-around',
            overflow: 'hidden',
            backgroundColor: theme.palette.background.paper,
        },
        gridList: {
            // width: '80vw',
            // height: '90vh',
        },
        icon: {
            color: 'rgba(255, 255, 255, 0.54)',
        },
    }));

    const classes = useStyles();

    useEffect(() => {
        generateUserDataList();

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        (!IsLoading && thisUserData !== undefined) ?
            <div className={classes.root}>

                {/* <GridList cellHeight={180} className={classes.gridList}>
                    <GridListTile key="Subheader" cols={2} style={{ height: 'auto' }}>
                        <ListSubheader component="div">Users</ListSubheader> */}

                <Grid container spacing={10}
                    style={{ padding: '24px' }}
                >
                    {users.map((userData) => ((!FilterforBlockedUsers(userData.id, thisUserData, userBlocks)) &&
                        <Grid key={userData.id} item
                            xs={12} sm={6} md={4} lg={4} xl={3}
                        >
                            <User key={userData.id} userData={userData} generateUserDataList={generateUserDataList} userBlocks={userBlocks} thisUser={thisUserData} />
                        </Grid>
                    ))}

                </Grid>
            </div>
            : null
    )

}

export default withRouter(UserDataList);