import React from 'react';

import { withStyles, makeStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import { grey } from '@material-ui/core/colors';


export function PrimaryButton(props) {


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

    const PrimaryButton = withStyles({
        root: {
            size: 'small',
            boxShadow: '5px 5px 5px #aaaaaa',
            textTransform: 'none',
            fontSize: 16,
            padding: '6px 12px',
            border: '1px solid',
            lineHeight: 1.5,
            backgroundColor: '#0063cc',
            color: grey[200],
            borderColor: '#0063cc',
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
        <PrimaryButton className={classes.ButtonDanger} onClick={props.handleClick}>{props.children}</PrimaryButton>
    )
}

export default PrimaryButton;