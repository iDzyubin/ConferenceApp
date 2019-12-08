import React, { useState } from 'react';
import styled from "styled-components";

import { SignIn } from '../services/api'
import { tokensStorage } from '../services/tokensStorage'
import useGlobal from '../store';

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: space-around;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
  width: 300px;
`;

const InputText = styled.input`
  display: flex;
  border-radius: 10px 10px 10px 10px;
  justify-content: space-around;
  width: 95%;
  color: black;
  margin-left: 20px;
  margin-right: 20px;
  padding-left: 20px;
  margin-bottom: 20px;
`;

const ButtonSendForm = styled.button`
 font-family: "Montserrat", sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: #5172bf;
  border: none;
  margin-bottom: 20px;
  padding: 11px 40px;
  transition: background-color 0.5s;

  :hover {
    background-color: #5172bf90;
    cursor: pointer;
  }
`;

const Title = styled.h2`
  font-weight: 400;
  text-align: center;
  color: #1d4dbb;
  font-size: 20px;
  line-height: 1.55;
  padding-top: 20px;
  margin-bottom: 35px;

  @media (min-width: 992px) {
    font-size: 38px;
    line-height: 1.39;
    max-width: 1100px;
    margin-left: auto;
    margin-right: auto;
    margin-bottom: 30px;
  }
`;

const Line = styled.hr`
  border: 1px solid #f1f1f1;
  margin-bottom: 25px;
`

const Form = styled.form``;
const FormGroup = styled.div``;
const LabelInput = styled.label`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  color: #1d4dbb;
`;

const Card = styled.div`
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  max-width: 800px;
  margin: auto;
  margin-top: 30px;
  text-align: center;
  font-family: arial;
`;

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  color: red;
`;

const InfoBlockError = styled.div`
  font-family: "Montserrat", sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: red;
  border: none;
  padding: 11px 40px;
  transition: background-color 0.5s;

  :hover {
    background-color: #5172bf90;
    cursor: pointer;
  }
`;

const Login = () => {
  const [globalState, globalActions] = useGlobal()
  const [Username, setUsername] = useState('');
  const [Password, setPassword] = useState('');

  const [authError, setAuthError] = useState(false);
  const [error, setError] = useState(false);

  const handleSubmit = async () => {
    const user = {
      Username,
      Password
    };
    SignIn(user)
      .catch(() => setError(true))
      .then(data => {
        if (checkRespone(data)) {
          setAuthError(false);
          tokensStorage.add(data);
          globalActions.setAuth(true)
        } else {
          setAuthError(true);
        }
      });
  }

  const checkRespone = (resp) => {
    return resp.hasOwnProperty('accessToken') && resp.hasOwnProperty('expires') && resp.hasOwnProperty('refreshToken');
  }

  const handleChangeUsername = (event) => {
    setUsername(event.target.value);
  }

  const handleChangePassword = (event) => {
    setPassword(event.target.value);
  }

  return (
    <Card>
      <Form>
        <FormGroup>
          <Title>Вход в режим администратора</Title>
          <Line />
          <LabelInput htmlFor="email"><b>Эл. адрес</b></LabelInput>
          <InputText type="text" placeholder="Введите эл. адрес" name="email" value={Username} onChange={handleChangeUsername} required />
          <LabelInput htmlFor="psw"><b>Пароль</b></LabelInput>
          <InputText type="password" placeholder="Введите пароль" name="psw" value={Password} onChange={handleChangePassword} required />
          <ButtonWrap>
            {error ? <InfoBlockError>Что-то пошло не так. Обратитесь к администратору</InfoBlockError> : <ButtonSendForm type="button" onClick={handleSubmit}>Вход</ButtonSendForm>}
          </ButtonWrap>
        </FormGroup>
        {authError && <InfoText>Неправильный логин или пароль</InfoText>}
      </Form>
    </Card>
  );
};

export default Login;