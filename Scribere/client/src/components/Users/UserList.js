import React, { useContext, useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { UserDataContext } from "../../providers/UserDataProvider";
import User from "./User";

import { makeStyles } from '@material-ui/core/styles';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import GridListTileBar from '@material-ui/core/GridListTileBar';
import ListSubheader from '@material-ui/core/ListSubheader';


const UserDataList = () => {

    const { users, getAllUsers } = useContext(UserDataContext);
    const [IsLoading, setIsLoading] = useState(true)


    const generateUserDataList = () => {
        getAllUsers();
        setIsLoading(false)
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
            width: '80vw',
            height: '90vh',
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
        (!IsLoading) ?
            <div className={classes.root}>

                <GridList cellHeight={180} className={classes.gridList}>
                    <GridListTile key="Subheader" cols={2} style={{ height: 'auto' }}>
                        <ListSubheader component="div">Users</ListSubheader>


                        {users.map((userData) => (
                            <User key={userData.id} userData={userData} generateUserDataList={generateUserDataList} />
                        ))}
                    </GridListTile>

                </GridList>
            </div>
            : null
    )

}

export default withRouter(UserDataList);