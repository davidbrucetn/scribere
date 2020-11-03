import React from 'react';

import { withStyles, makeStyles } from '@material-ui/core/styles';
import red from '@material-ui/core/colors/red';
import Button from '@material-ui/core/Button';
import { grey } from '@material-ui/core/colors';


export function DangerButton(props) {

    const secondary = red.A700;

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
            backgroundColor: secondary,
            color: grey[200]
        },
        ButtonPrimary: {

        }

    });

    const classes = useStyles();

    const DangerButton = withStyles({
        root: {
            size: 'small',
            margin: '1em 1em',
            boxShadow: '3px 3px 3px #aaaaaa',
            textTransform: 'none',
            fontSize: 16,
            padding: '6px 12px',
            border: '1px solid',
            lineHeight: 1.5,
            backgroundColor: '#d50000',
            borderColor: '#d50000',
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
            '&:hover': {
                backgroundColor: 'inherit',
                borderColor: '#0062cc',
            },
            '&:active': {
                boxShadow: 'none',
                backgroundColor: '#0062cc',
                borderColor: '#005cbf',
            },
            '&:focus': {
                boxShadow: '0 0 0 0.2rem rgba(0,123,255,.5)',
            },
        },
    })(Button);

    // End Material-UI styling 

    return (
        <DangerButton className={classes.ButtonDanger} onClick={props.handleClick}>{props.children}</DangerButton>
    )
}

export default DangerButton;