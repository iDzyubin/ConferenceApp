import React, { useState } from 'react';
import styled from 'styled-components';

import * as Api from '../services/api';

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: space-around;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
  width: 500px;
`;

const Button = styled.button`
  font-family: 'Montserrat', sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: #5172bf;
  border: none;
  margin-bottom: 20px;
  margin-right: 5px;
  padding: 11px 40px;
  transition: opacity 0.5s;

  :hover {
    opacity: 0.5;
    cursor: pointer;
  }
`;

const ModalWindow = styled.div`
  position: fixed;
  z-index: 1;
  padding-top: 100px;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgb(0, 0, 0);
  background-color: rgba(0, 0, 0, 0.4);
`;

const ModalContent = styled.div`
  background-color: #fefefe;
  margin: auto;
  padding: 20px;
  border: 1px solid #888;
  width: 80%;
`;

const ModalCloseButton = styled.span`
  color: #aaaaaa;
  float: right;
  margin-top: -15px;
  font-size: 28px;
  font-weight: bold;

  :hover {
    color: #000;
    text-decoration: none;
    cursor: pointer;
  }

  :focus {
    color: #000;
    text-decoration: none;
    cursor: pointer;
  }
`;

const LabelInput = styled.label`
  font-size: 20px;
  color: #5172bf;
`;

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 10px;
  color: red;
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

const LoadModalWindow = styled.div`
  position: fixed;
  z-index: 2;
  padding-top: 100px;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgb(0, 0, 0);
  background-color: rgba(0, 0, 0, 0.4);
`;

const Form = styled.form``;
const FormGroup = styled.div``;

const EditUser = props => {
  const [view, setView] = useState(false);

  const [FirstName, setFirstName] = useState();
  const [MiddleName, setMiddleName] = useState('');
  const [LastName, setLastName] = useState('');
  const [Organisation, setOrganization] = useState('');
  const [Phone, setPhone] = useState('');
  const [OrganisationAddress, setOrganizationAddress] = useState('');
  const [City, setCity] = useState('');
  const [Position, setPosition] = useState('');

  const [load, setLoad] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async e => {
    let formObj = document.getElementById('edit-user-form');
    if (formObj.checkValidity()) {
      const user = {
        id: props.userId,
        FirstName,
        MiddleName,
        LastName,
        Organisation,
        Phone,
        OrganisationAddress,
        City,
        Position
      };
      setLoad(true);
      Api.UpdateUser(props.userId, props.token, user)
        .catch(() => {
          setLoad(false);
          setError('Ошибка изменения информации о пользователе');
        })
        .then(response => {
          if (response) {
            setError(null);
            props.editFIO(`${LastName} ${FirstName} ${MiddleName}`);
            setView(!view);
          } else {
            setError('Ошибка изменения информации о пользователе');
          }
          setLoad(false);
        });
    }
  };

  const fields = [
    {
      key: 'FirstName',
      str: 'Имя',
      value: FirstName,
      handler: setFirstName,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'MiddleName',
      str: 'Отчество',
      value: MiddleName,
      handler: setMiddleName,
      required: false,
      type: 'text',
      minlength: 1
    },
    {
      key: 'LastName',
      str: 'Фамилия',
      value: LastName,
      handler: setLastName,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'Organisation',
      str: 'Организация',
      value: Organisation,
      handler: setOrganization,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'Phone',
      str: 'Телефон',
      value: Phone,
      handler: setPhone,
      required: true,
      type: 'number',
      minlength: 1
    },
    {
      key: 'OrganisationAddress',
      str: 'Адрес организации',
      value: OrganisationAddress,
      handler: setOrganizationAddress,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'City',
      str: 'Город организации',
      value: City,
      handler: setCity,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'Position',
      str: 'Должность',
      value: Position,
      handler: setPosition,
      required: false,
      type: 'text',
      minlength: 1
    }
  ];

  const toggleModalWindow = () => {
    if (!view) {
      setLoad(true);
      Api.GetUser(props.userId, props.token)
        .catch(() => {
          setLoad(false);
          setError('Ошибка получения информации о пользователе');
        })
        .then(r => {
          if (r) {
            setError(null);
            setFirstName(r.firstName ? r.firstName : '');
            setMiddleName(r.middleName ? r.middleName : '');
            setLastName(r.lastName ? r.lastName : '');
            setOrganization(r.organisation ? r.organisation : '');
            setPhone(r.phone ? r.phone : '');
            setOrganizationAddress(
              r.organisationAddress ? r.organisationAddress : ''
            );
            setCity(r.city ? r.city : '');
            setPosition(r.position ? r.position : '');
          } else {
            setError('Ошибка получения информации о пользователе');
          }
          setLoad(false);
        });
    }
    setView(!view);
  };

  const getModalState = state => {
    return { display: state ? 'block' : 'none' };
  };

  const handleTextInput = event => {
    const field = fields.find(f => f.key === event.target.id);
    field.handler(event.target.value);
  };

  return (
    <div>
      {load ? (
        <LoadModalWindow style={getModalState(load)}>
          <div className="spinner-border text-light" role="status">
            <span className="sr-only">Loading...</span>
          </div>
        </LoadModalWindow>
      ) : null}
      <Button type="button" onClick={() => toggleModalWindow()}>
        Изменить данные о себе
      </Button>
      <ModalWindow style={getModalState(view)}>
        <ModalContent>
          <ModalCloseButton onClick={() => setView(!view)}>
            &times;
          </ModalCloseButton>
          <Form id="edit-user-form" onSubmit={e => e.preventDefault()}>
            <FormGroup>
              {fields.map(f => (
                <div key={f.key}>
                  <LabelInput>{f.str}</LabelInput>
                  <InputText
                    placeholder={f.str}
                    id={f.key}
                    value={f.value}
                    onChange={handleTextInput}
                    required={f.required}
                    type={f.type}
                  />
                </div>
              ))}
              <ButtonWrap>
                <Button type="submit" onClick={handleSubmit}>
                  Изменить данные
                </Button>
              </ButtonWrap>
              {error && <InfoText>{error}</InfoText>}
            </FormGroup>
          </Form>
        </ModalContent>
      </ModalWindow>
    </div>
  );
};

export default EditUser;
