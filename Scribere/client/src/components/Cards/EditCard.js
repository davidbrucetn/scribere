import React, { useContext, useState } from 'react';

import { withStyles, makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';


export function EditCard(props) {


    const useStyles = makeStyles({
        card: {
            borderRadius: 2,
            backgroundColor: 'default',
            paddingBottom: 5,
            marginBottom: 3,
            marginTop: 3,
            paddingTop: 3
        },
        media: {
            height: 140,
        },
        ButtonDanger: {
        },
        ButtonPrimary: {

        }

    });

    const classes = useStyles();

    const EditCard = withStyles({
        root: {
            size: 'small',
            boxShadow: '10px 10px 5px #aaaaaa',
            textTransform: 'none',
            fontSize: 16,
            padding: '6px 12px',
            lineHeight: 1.5,
            backgroundColor: '#f2f3f3',
            fontFamily: [
                '-apple-system',
                'BlinkMacSystemFont',
                '"Segoe UI"',
                'Roboto',
                '"Helvetica Neue"',
                'Arial',
                'sans-serif',
                '"Apple Color Emoji"',
                '"Segoe UI Emoji"',
                '"Segoe UI Symbol"',
            ].join(','),

        },
    })(Card);

    // End Material-UI styling 

    return (
        <EditCard className={classes.ButtonDanger} onClick={props.handleClick} >{props.children}</EditCard>
    )
}

export default EditCard;