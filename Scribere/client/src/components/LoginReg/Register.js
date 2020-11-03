import React, { useState, useContext } from "react";
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import { useHistory, Link } from "react-router-dom";
import { UserDataContext } from "../../providers/UserDataProvider";

import "./LoginReg.css";

import Container from '@material-ui/core/Container';
import Paper from "@material-ui/core/Paper";
import { makeStyles } from '@material-ui/core/styles';



export default function Register() {
  const history = useHistory();
  const { register } = useContext(UserDataContext);

  const [nameFirst, setFirstName] = useState();
  const [nameLast, setLastName] = useState();
  const [pseudonym, setPseudonym] = useState();
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const [imageUrl, setImageUrl] = useState();
  const [confirmPassword, setConfirmPassword] = useState();

  const useStyles = makeStyles((theme) => ({

    containerRegistration: {
      width: 'auto',
      height: 'auto',
      opacity: 1,
      animationName: 'fadeInOpacity',
      animationTimingFunction: 'ease',
      animationDuration: '3s',
      paddingTop: 3,

    },
    paper: {
      borderRadius: '3em',

    },
    formRegistration: {
      borderRadius: '3em',
      width: 'auto',
      backgroundColor: 'gray[200]',
      padding: '2em',
      marginTop: '20',
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

  const registerClick = (e) => {
    e.preventDefault();
    if (password && password !== confirmPassword) {
      alert("Passwords don't match. Do better.");
    } else {
      const userData = { nameLast, nameFirst, pseudonym, email };
      if (imageUrl !== "") {
        userData.userImage = {
          "imageUrl": imageUrl
        }
      }
      register(userData, password)
        .then(() => history.push("/"));
    }
  };

  return (
    <div className="container__login">

      <div className="div__registration">

        <Container className={classes.containerRegistration}>
          <Paper className={classes.paper}>
            <Form className={classes.formRegistration} onSubmit={registerClick}>
              <fieldset>
                <FormGroup>
                  <Label htmlFor="firstName">First Name</Label>
                  <Input id="nameFirst" type="text" size={50} onChange={e => setFirstName(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Label htmlFor="lastName">Last Name</Label>
                  <Input id="nameLast" type="text" size={50} onChange={e => setLastName(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Label htmlFor="pseudonym">Pseudonym</Label>
                  <Input id="pseudonym" type="text" size={50} onChange={e => setPseudonym(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Label htmlFor="imageUrll">User Image URL</Label>
                  <Input id="imageUrl" type="text" onChange={e => setImageUrl(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Label for="email">Email</Label>
                  <Input id="email" type="text" onChange={e => setEmail(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Label for="password">Password</Label>
                  <Input id="password" type="password" onChange={e => setPassword(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Label for="confirmPassword">Confirm Password</Label>
                  <Input id="confirmPassword" type="password" onChange={e => setConfirmPassword(e.target.value)} />
                </FormGroup>
                <FormGroup>
                  <Button className={classes.buttonSave}>Register</Button>
                </FormGroup>
                <em>
                  Already Registered? <Link to="login">Login</Link>
                </em>
              </fieldset>
            </Form>
          </Paper>
        </Container>
      </div>
    </div>
  );
}
