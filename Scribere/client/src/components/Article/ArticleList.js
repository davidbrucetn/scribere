import React, { useContext, useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { ArticleContext } from "../../providers/ArticleProvider";
import Article from "./Article";

import { makeStyles } from '@material-ui/core/styles';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import GridListTileBar from '@material-ui/core/GridListTileBar';
import ListSubheader from '@material-ui/core/ListSubheader';


const ArticleList = () => {

    const { articles, getAllArticles } = useContext(ArticleContext);
    const [IsLoading, setIsLoading] = useState(true)


    const generateArticleList = () => {
        getAllArticles();
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
        generateArticleList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        (!IsLoading) ?
            <div className={classes.root}>

                <GridList cellHeight={180} className={classes.gridList}>
                    <GridListTile key="Subheader" cols={2} style={{ height: 'auto' }}>
                        <ListSubheader component="div">Articles</ListSubheader>


                        {articles.map((article) => (
                            <Article key={article.id} article={article} generateArticleList={generateArticleList} />
                        ))}
                    </GridListTile>

                </GridList>
            </div>
            : null
    )

}

export default withRouter(ArticleList);