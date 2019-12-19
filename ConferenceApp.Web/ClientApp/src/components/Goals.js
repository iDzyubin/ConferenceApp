import React from 'react';

import styled from 'styled-components';

import growIcon from '../assets/img/grow.png';
import moneyIcon from '../assets/img/money.png';
import socialIcon from '../assets/img/social.png';

const Section = styled.section`
  background-color: #748ecb;
  padding-top: 30px;
  padding-bottom: 30px;
`;

const Icon = styled.img`
  height: 120px;
  padding-top: 30px;
`;

const SectionTitle = styled.h2`
  font-size: 26px;
  font-weight: 400;
  line-height: normal;
  color: #fff;
  text-align: center;
  margin-bottom: 30px;
`;

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

const IconWrap = styled.div`
  width: 150px;
  height: 150px;
  border-radius: 150px;
  background-color: #c7d1ea;
  margin-bottom: 30px;
  position: relative;
  > svg {
    transition: all 0.3s ease-in;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
  }
  &:hover {
    > svg {
      transform: translate(-50%, -50%) rotateY(360deg);
    }
  }
`;

const BoxTitle = styled.h4`
  font-size: 18px;
  font-weight: 300;
  line-height: normal;
  color: #fff;
`;

const Text = styled.p`
  font-size: 12px;
  font-weight: normal;
  line-height: 1.58;
  color: #8f8f8f;
  margin-bottom: 0;
  max-width: 350px;
`;

const Goals = () => {
  const width = window.innerWidth;
  return (
    <Section id='goals'>
      <div className='container'>
        <SectionTitle>Конференция проводится в целях:</SectionTitle>
        <div className='row'>
          <div className='col-lg-4'>
            <Box data-aos={width >= 1400 ? 'fade-right' : 'fade-up'}>
              <IconWrap>
                <Icon src={growIcon} />
              </IconWrap>
              <BoxTitle>
                Развития научного и творческого потенциала молодых
                исследователей в области программной инженерии
              </BoxTitle>
            </Box>
          </div>
          <div className='col-lg-4'>
            <Box data-aos='fade-up'>
              <IconWrap>
                <Icon src={socialIcon} />
              </IconWrap>
              <BoxTitle>
                Активизации процесса обмена новыми идеями, и стимулирования
                творческого мышления среди молодежи
              </BoxTitle>
            </Box>
          </div>
          <div className='col-lg-4'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <IconWrap>
                <Icon src={moneyIcon} />
              </IconWrap>
              <BoxTitle>
                Формирования кадрового резерва для российских IT компаний
              </BoxTitle>
            </Box>
          </div>
        </div>
      </div>
    </Section>
  );
};

export default Goals;
