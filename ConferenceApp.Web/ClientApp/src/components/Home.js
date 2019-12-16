import React from 'react';

import styled from 'styled-components';

import bgImg from '../assets/img/bg-home.jpg';
import bgOver from '../assets/img/bgover.jpg';
import rector from '../assets/img/rector.jpg';

const Section = styled.section`
  position: relative;
  padding-top: 138px;
  padding-bottom: 30px;
  background-repeat: no-repeat;
  background-size: cover;
  background-position: center;
  background-attachment: fixed;
  background-image: url(${bgImg});

  &:before {
    left: 0;
    border-right: 12px solid transparent;
    border-left: calc(50vw - 12px) solid #fff;
  }

  @media (min-width: 992px) {
    padding-left: 110px;
    padding-right: 110px;
    padding-top: 200px;
    padding-bottom: 111px;
  }

  .container {
    z-index: 1;
    user-select: none;
    cursor: default;
  }
`;

const SubSection = styled.div`
  display: flex;
  justify-content: center;
  flex-flow: column;
`;

const BgOverlay = styled.div`
  background-image: url(${bgOver});
  filter: brightness(70%);
  opacity: 0.7;
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

const HomeTitle = styled.h1`
  font-weight: 500;
  text-align: left;
  color: #fff;
  font-size: 50px;
  line-height: 1;
  margin-top: 30px;
  margin-bottom: 30px;
  @media (min-width: 992px) {
    font-size: 55px;
    line-height: 1;
    max-width: 1100px;
    margin-top: 50px;
    margin-bottom: 30px;
    margin-left: auto;
    margin-right: auto;
  }
`;

const HomeSubTitle = styled.p`
  font-weight: 400;
  text-align: left;
  color: #fff;
  font-size: 15px;
  line-height: 1.39;
  margin-bottom: 35px;
  @media (min-width: 992px) {
    font-weight: 400;
    font-size: 20px;
    line-height: 1.39;
    max-width: 1100px;
    margin-left: auto;
    margin-right: auto;
    margin-bottom: 30px;
  }
`;

const Title = styled.h2`
  font-weight: 500;
  text-align: center;
  color: #2b55bc;
  font-size: 20px;
  line-height: 1;
  margin-top: 30px;
  margin-bottom: 30px;
  @media (min-width: 992px) {
    font-size: 25px;
    line-height: 1;
    max-width: 1100px;
    margin-top: 50px;
    margin-bottom: 30px;
    margin-left: auto;
    margin-right: auto;
  }
`;

const SubTitle = styled.h5`
  font-size: 18px;
  font-weight: normal;
  line-height: 1.83;
  text-align: left;
  color: #000;
  @media (min-width: 992px) {
    max-width: 785px;
    margin-left: auto;
    margin-right: auto;
    margin-bottom: 49px;
  }
`;

const Rector = styled.img`
  width: 280px;
  height: 400px;
  float: left;
  margin-right: 20px;
`;

const Home = () => {
  return (
    <div>
      <Section id='home'>
        <div className='container'>
          <BgOverlay />
          <HomeTitle data-aos='zoom-in'>
            ПРОГРАММНАЯ
            <br />
            ИНЖЕНЕРИЯ:
            <HomeSubTitle>
              СОВРЕМЕННЫЕ ТЕНДЕНЦИИ РАЗВИТИЯ И ПРИМЕНЕНИЯ (ПИ-2020)
            </HomeSubTitle>
          </HomeTitle>
        </div>
      </Section>
      <SubSection>
        <Title data-aos='zoom-in'>Приветствие участникам конференции</Title>
        <SubTitle
          data-aos='fade-up'
          data-aos-easing='ease'
          data-aos-delay='400'>
          <Rector src={rector} />
          Приглашаем Вас принять участие в работе IV Всероссийской
          научно-практической конференции
          <br />
          <b>
            «ПРОГРАММНАЯ ИНЖЕНЕРИЯ: СОВРЕМЕННЫЕ ТЕНДЕНЦИИ РАЗВИТИЯ И ПРИМЕНЕНИЯ
            (ПИ-2020)»
          </b>
          <br />, которая состоится 11-12 марта 2020 года в федеральном
          государственном бюджетном образовательном учреждении высшего
          образования «Юго-Западный государственный университет».
        </SubTitle>
      </SubSection>
    </div>
  );
};

export default Home;
