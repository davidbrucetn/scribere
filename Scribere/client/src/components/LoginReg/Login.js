import React, { useState, useContext } from "react";
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import { useHistory, Link } from "react-router-dom";
import { UserDataContext } from "../../providers/UserDataProvider";

import "./LoginReg.css";

import Container from '@material-ui/core/Container';
import Paper from "@material-ui/core/Paper";
import { makeStyles } from '@material-ui/core/styles';

export default function Login() {
  const history = useHistory();
  const { login } = useContext(UserDataContext);

  const [email, setEmail] = useState();
  const [password, setPassword] = useState();

  const useStyles = makeStyles((theme) => ({
    root: {
      display: 'flex',
      flexFlow: 'column wrap',
      justifyContent: 'space-around',
      overflow: 'Hidden',
      backgroundColor: theme.palette.background.paper,
    },
    paper: {
      borderRadius: '3em',

    },
    containerRegistration: {
      width: 'auto',
      height: 'auto',
      backgroundColor: 'gray[200]',
      opacity: 1,
      animationName: 'fadeInOpacity',
      animationTimingFunction: 'ease',
      animationDuration: '2s',
      paddingTop: 10,

    },
    formRegistration: {
      borderRadius: '3em',
      width: 'auto',
      backgroundColor: 'gray[200]',
      padding: '1em',
      boxShadow: '10px 10px 15px #aaaaaa'
    },
    icon: {
      color: 'rgba(255, 255, 255, 0.54)',
    },
    buttonSave: {
      backgroundColor: '#0063cc',
    },
    buttonDelete: {
      backgroundColor: '#d50000',
      size: 'small',
    },
    Typography: {
      fontFamily: [
        'Merriweather',
        'serif'
      ].join(','),
      whiteSpace: 'pre-line'
    },
    card: {
      margin: '0 1em',
    }
  }));

  const classes = useStyles();


  const loginSubmit = (e) => {
    e.preventDefault();
    login(email, password)
      .then(() => history.push("/"))
      .catch(() => alert("Invalid email or password"));
  };

  return (
    <div className="container__login">

      <div className="div__registration">

        <Container className={classes.containerRegistration}>
          <Paper className={classes.paper}>
            <Form className={classes.formRegistration} onSubmit={loginSubmit} >
              <fieldset>
                <FormGroup>
                  <Label for="email">Email</Label>
                  <Input id="email" type="text" size={50} onChange={e => setEmail(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Label for="password">Password</Label>
                  <Input id="password" type="password" onChange={e => setPassword(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Button>Login</Button>
                </FormGroup>
                <em>
                  Not registered? <Link to="register">Register</Link>
                </em>
              </fieldset>
            </Form>
          </Paper>
        </Container>
      </div>
    </div>
  );
}