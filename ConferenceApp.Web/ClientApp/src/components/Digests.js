import React from 'react';
import styled from 'styled-components';

import BookImg from '../assets/img/book.jpg';
import bgOver from '../assets/img/bgover.jpg';

const Section = styled.section`
  padding-top: 30px;
  padding-bottom: 30px;
  position: relative;
  height: 100%;
  background-color: #fff;
`;

const BgOverlay = styled.div`
  background-image: url(${bgOver});
  filter: brightness(70%);
  opacity: 0.2;
  background-repeat: no-repeat;
  background-size: cover;
  background-position-y: 50%;
  position: absolute;
  height: 100%;
  width: 100%;
  right: 0;
  bottom: 0;
  left: 0;
  top: 0;
  @media (min-width: 992px) {
    background-image: url(${bgOver});
  }
`;

const SectionTitle = styled.h2`
  font-size: 26px;
  font-weight: 400;
  line-height: normal;
  color: #000;
  text-align: center;
  padding-top: 10px;
  @media (min-width: 992px) {
    margin-bottom: 50px;
  }
`;

const Card = styled.div`
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  max-width: 300px;
  background-color: #808080;
  margin: auto;
  text-align: center;
  font-family: arial;
`;

const SubTitle = styled.p`
  color: white;
  font-size: 22px;
`;

const CardButton = styled.button`
  border: none;
  outline: 0;
  padding: 12px;
  color: white;
  background-color: #5172bf;
  text-align: center;
  cursor: pointer;
  width: 100%;
  font-size: 18px;

  :hover {
    opacity: 0.7;
  }
`;

const BookImage = styled.img`
  width: 100%;
`;

const DateTitle = styled.h3``;

const Description = styled.p``;

const Box = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  margin-bottom: 30px;
  &:hover {
    transform: translateY(-5px) !important;
  }

  @media (min-width: 992px) {
    margin-bottom: 0;
  }
`;

const WrapBox = styled.div`
  margin-bottom: 30px;
`;

const Digests = () => {
  return (
    <Section id='digests'>
      <BgOverlay />
      <div className='container'>
        <SectionTitle>Сборники прошлых лет</SectionTitle>
        <div className='row'>
          <WrapBox className='col-lg-4'>
            <Box>
              <Card>
                <BookImage src={BookImg} alt='BOOK' />
                <DateTitle>2017</DateTitle>
                <SubTitle>Подпись</SubTitle>
                <Description>
                  Описание Описание Описание Описание Описание Описание Описание
                  Описание Описание Описание Описание Описание{' '}
                </Description>
                <CardButton>Скачать</CardButton>
              </Card>
            </Box>
          </WrapBox>
          <WrapBox className='col-lg-4'>
            <Box>
              <Card>
                <BookImage src={BookImg} alt='BOOK' />
                <DateTitle>2018</DateTitle>
                <SubTitle>Подпись</SubTitle>
                <Description>
                  Описание Описание Описание Описание Описание Описание Описание
                  Описание Описание Описание Описание Описание{' '}
                </Description>
                <CardButton>Скачать</CardButton>
              </Card>
            </Box>
          </WrapBox>
          <WrapBox className='col-lg-4'>
            <Box>
              <Card>
                <BookImage src={BookImg} alt='BOOK' />
                <DateTitle>2019</DateTitle>
                <SubTitle>Подпись</SubTitle>
                <Description>
                  Описание Описание Описание Описание Описание Описание Описание
                  Описание Описание Описание Описание Описание{' '}
                </Description>
                <CardButton>Скачать</CardButton>
              </Card>
            </Box>
          </WrapBox>
        </div>
      </div>
    </Section>
  );
};

export default Digests;
