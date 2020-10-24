import React, { useState, useContext } from "react";
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import { useHistory } from "react-router-dom";
import { UserDataContext } from "../providers/UserDataProvider";
export default function Register() {
  const history = useHistory();
  const { register } = useContext(UserDataContext);

  const [nameFirst, setFirstName] = useState();
  const [nameLast, setLastName] = useState();
  const [pseudonym, setPseudonym] = useState();
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const [confirmPassword, setConfirmPassword] = useState();
  const userLevelId = 2;

  const registerClick = (e) => {
    e.preventDefault();
    if (password && password !== confirmPassword) {
      alert("Passwords don't match. Do better.");
    } else {
      const userData = { nameLast, nameFirst, pseudonym, email, userLevelId };

      register(userData, password)
        .then(() => history.push("/"));
    }
  };

  return (
    <Form onSubmit={registerClick}>
      <fieldset>
        <FormGroup>
          <Label htmlFor="firstName">First Name</Label>
          <Input id="nameFirst" type="text" onChange={e => setFirstName(e.target.value)} />
        </FormGroup>
        <FormGroup>
          <Label htmlFor="lastName">Last Name</Label>
          <Input id="nameLast" type="text" onChange={e => setLastName(e.target.value)} />
        </FormGroup>
        <FormGroup>
          <Label htmlFor="pseudonym">Pseudonym</Label>
          <Input id="pseudonym" type="text" onChange={e => setPseudonym(e.target.value)} />
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
          <Button>Register</Button>
        </FormGroup>
      </fieldset>
    </Form>
  );
}
