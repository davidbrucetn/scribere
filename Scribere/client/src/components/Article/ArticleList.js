import React, { useContext, useEffect, useState } from "react";
import { withRouter, useParams } from "react-router-dom";
import { ArticleContext } from "../../providers/ArticleProvider";
import { UserBlockContext } from "../../providers/UserBlockProvider";
import { FilterforBlockedUsers } from "../Helper/DWBUtils";
import Article from "./Article";
import "./ArticleList.css";

import { makeStyles } from '@material-ui/core/styles';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import ListSubheader from '@material-ui/core/ListSubheader';
import { Hidden } from "@material-ui/core";

let thisUserData = null;
let listFilter = "All";
const ArticleList = (props) => {
    const [IsLoading, setIsLoading] = useState(true);
    const [articles, setArticles] = useState([]);
    const { userBlocks, getAllUserBlocks } = useContext(UserBlockContext);
    const { getAllArticles, getFavoriteArticles, getMyArticles } = useContext(ArticleContext);

    const params = useParams();

    thisUserData = JSON.parse(sessionStorage.UserData);

    const generateArticleList = () => {
        getAllUserBlocks()
        if (params.favorites === "favarticles") {
            getFavoriteArticles()
                .then((articleResp) => {
                    setArticles([...articleResp])
                })

        } else {
            if (params.mywork === "mywork") {
                listFilter = "Self";
                getMyArticles()
                    .then((articleResp) => {
                        setArticles([...articleResp])
                    })
            } else if (params.categoryType !== undefined) {
                listFilter = params.categoryType;
            } else {
                listFilter = "All"
            }
            getAllArticles(listFilter)
                .then((articleResp) => {
                    setArticles([...articleResp])
                })
        }
        setIsLoading(false)
    }

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

        },
        icon: {
            color: 'rgba(255, 255, 255, 0.54)',
        },
    }));

    const classes = useStyles();


    useEffect(() => {
        generateArticleList();



        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [props.location.pathname]);

    return (
        (!IsLoading && thisUserData !== null) ?
            <div className={classes.root}>

                <GridList cellHeight={180} className={classes.gridList}>
                    <GridListTile key="Subheader" cols={2} style={{ height: 'auto' }}>
                        <ListSubheader component="div"></ListSubheader>


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