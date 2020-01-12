import React, { useState, useEffect } from 'react';
import * as Api from '../services/api';
import styled from 'styled-components';

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  font-size: 20px;
  color: #5172bf;
`;

const Table = styled.table`
  border-collapse: collapse;
  width: 95%;
  margin-left: auto;
  margin-right: auto;
`;

const TableData = styled.td`
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
`;

const TableHeader = styled.th`
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
`;

const TableRow = styled.tr`
  :hover {
    background-color: #cccccc;
  }
`;

const TableWrap = styled.div`
  margin-bottom: 10px;
`;

const InputText = styled.input`
  flex: 3;
  border-radius: 10px 10px 10px 10px;
  justify-content: left;
  width: 65%;
  color: black;
  margin-left: 20px;
  margin-right: 20px;
  padding-left: 20px;
  margin-bottom: 20px;
`;

const MiniButton = styled.button`
  font-family: 'Montserrat', sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  border: none;
  margin-right: 5px;
  margin-bottom: 5px;
  transition: opacity 0.5s;

  :hover {
    opacity: 0.5;
    cursor: pointer;
  }
`;

const AddSectionForm = props => {
  const initialFormState = { id: null, title: '' };
  const [section, setSection] = useState(initialFormState);

  const handleInputChange = event => {
    const { value } = event.currentTarget;
    setSection({ ...section, title: value });
  };

  const handleSubmit = event => {
    event.preventDefault();
    if (!section.title) return;
    props.addSection(section);
    setSection(initialFormState);
  };

  return (
    <form onSubmit={handleSubmit}>
      <InputText
        type="text"
        name="title"
        placeholder="Название"
        value={section.title}
        onChange={handleInputChange}
      />
      <MiniButton
        style={{
          backgroundColor: 'steelblue'
        }}
      >
        Добавить новую секцию
      </MiniButton>
    </form>
  );
};

const EditSectionForm = props => {
  const [section, setSection] = useState(props.currentSection);

  useEffect(() => {
    setSection(props.currentSection);
  }, [props]);

  const handleInputChange = event => {
    const { value } = event.target;
    setSection({ ...section, title: value });
  };

  const handleSubmit = event => {
    event.preventDefault();
    if (!section.title) return;
    props.updateSection(section.id, section);
  };

  return (
    <form onSubmit={handleSubmit}>
      <InputText
        type="text"
        name="title"
        placeholder="Название"
        value={section.title}
        onChange={handleInputChange}
      />
      <MiniButton
        style={{
          backgroundColor: 'steelblue'
        }}
      >
        Изменить секцию
      </MiniButton>
      <MiniButton
        style={{
          backgroundColor: 'grey'
        }}
        onClick={() => props.setEditing(false)}
        className="button muted-button"
      >
        Отмена
      </MiniButton>
    </form>
  );
};

const SectionTable = props => {
  const handleDeleteSection = id => {
    let answer = window.confirm('Вы уверены?');

    if (answer) {
      props.deleteSection(id);
    }
  };
  return (
    <Table>
      <thead>
        <TableRow>
          <TableHeader>Название</TableHeader>
          <TableHeader>Действия</TableHeader>
        </TableRow>
      </thead>
      <tbody>
        {props.sections.length > 0 ? (
          props.sections.map(section => (
            <TableRow key={section.id}>
              <TableData>{section.title}</TableData>
              <TableData>
                <MiniButton
                  style={{
                    backgroundColor: 'steelblue'
                  }}
                  onClick={() => {
                    props.editRow(section);
                  }}
                  className="button muted-button"
                >
                  Изменить
                </MiniButton>
                <MiniButton
                  style={{
                    backgroundColor: 'red'
                  }}
                  className="button muted-button"
                  onClick={() => handleDeleteSection(section.id)}
                >
                  Удалить
                </MiniButton>
              </TableData>
            </TableRow>
          ))
        ) : (
          <TableRow>
            <TableData colSpan={3}>Вы не добавили ни одной секции</TableData>
          </TableRow>
        )}
      </tbody>
    </Table>
  );
};

const Sections = props => {
  const [sections, setSections] = useState(props.sections);
  const [editing, setEditing] = useState(false);
  const initialFormState = { id: null, title: '' };
  const [currentSection, setCurrentSection] = useState(initialFormState);

  const [error, setError] = useState(null);

  const addSection = section => {
    Api.CreateSection(props.token, { title: section.title })
      .catch(() => setError('При добавлении секций возникла ошибка'))
      .then(data => {
        if (data) {
          section.id = data.id;
          setSections([...sections, section]);
          props.setSections(sections);
        } else {
          setError('При добавлении секций возникла ошибка');
        }
      });
  };

  const deleteSection = id => {
    Api.DeleteSection(props.token, id)
      .catch(() => setError('При удалении секций возникла ошибка'))
      .then(data => {
        if (data) {
          setEditing(false);
          setSections(sections.filter(section => section.id !== id));
          props.setSections(sections);
        } else {
          setError('При удалении секций возникла ошибка');
        }
      });
  };

  const updateSection = (id, updatedSection) => {
    Api.UpdateSection(props.token, updatedSection)
      .catch(() => setError('При изменении секций возникла ошибка'))
      .then(data => {
        if (data) {
          setEditing(false);
          setSections(
            sections.map(section =>
              section.id === id ? updatedSection : section
            )
          );
          props.setSections(sections);
        } else {
          setError('При изменении секций возникла ошибка');
        }
      });
  };

  const editRow = section => {
    setEditing(true);
    setCurrentSection({ id: section.id, title: section.title });
  };

  return (
    <div className="container">
      <div className="flex-row">
        <div className="flex-large">
          {editing ? (
            <div>
              <InfoText>Редактирование секции</InfoText>
              <EditSectionForm
                editing={editing}
                setEditing={setEditing}
                currentSection={currentSection}
                updateSection={updateSection}
              />
            </div>
          ) : (
            <div>
              <InfoText>Добавить секцию</InfoText>
              <AddSectionForm addSection={addSection} />
            </div>
          )}
          <div className="flex-large">
            <InfoText>Список всех секций</InfoText>
            <TableWrap>
              <SectionTable
                sections={sections}
                editRow={editRow}
                deleteSection={deleteSection}
              />
            </TableWrap>
          </div>
        </div>
      </div>
      {error && <InfoText>{error}</InfoText>}
    </div>
  );
};

export { Sections };
