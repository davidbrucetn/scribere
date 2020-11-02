import React, { useContext, useEffect, useState } from "react";
import { withRouter, useParams } from "react-router-dom";
import { ArticleContext } from "../../providers/ArticleProvider";
import { UserDataContext } from "../../providers/UserDataProvider";
import { UserBlockContext } from "../../providers/UserBlockProvider";
import { FilterforBlockedUsers } from "../Helper/DWBUtils";
import Article from "./Article";
import "./ArticleList.css";

import { makeStyles } from '@material-ui/core/styles';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import GridListTileBar from '@material-ui/core/GridListTileBar';
import ListSubheader from '@material-ui/core/ListSubheader';
import { Hidden } from "@material-ui/core";

let thisUserData = null;
let listFilter = "All";
const ArticleList = (props) => {
    const [IsLoading, setIsLoading] = useState(true)
    const { userBlocks, addUserBlock, getAllUserBlocks, deleteUserBlock } = useContext(UserBlockContext);
    const { articles, getAllArticles } = useContext(ArticleContext);
    const [articlesList, setArticlesList] = useState([]);


    const params = useParams();

    thisUserData = JSON.parse(sessionStorage.UserData);



    const generateArticleList = () => {
        if (params.mywork === "mywork") {
            listFilter = "Self";
        } else if (params.categoryType !== undefined) {
            listFilter = params.categoryType;
        }
        Promise.all([getAllArticles(listFilter), getAllUserBlocks()]);
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
        if (articles !== [...articles]) {
            generateArticleList();
        }


        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [props.location.pathname, listFilter]);

    return (
        (!IsLoading && thisUserData !== null) ?
            <div className={classes.root}>

                <GridList cellHeight={180} className={classes.gridList}>
                    <GridListTile key="Subheader" cols={2} style={{ height: 'auto' }}>
                        <ListSubheader component="div">Articles</ListSubheader>


                        {articles.map((article) => ((!FilterforBlockedUsers(article.userId, thisUserData, userBlocks)) &&
                            <Article key={article.id} article={article} generateArticleList={generateArticleList}
                            />
                        ))}
                    </GridListTile>

                </GridList>
            </div>
            : null
    )

}

export default withRouter(ArticleList);