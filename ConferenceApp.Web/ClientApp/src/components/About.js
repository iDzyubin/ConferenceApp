import React from 'react';

import styled from 'styled-components';

import anikinaPhoto from '../assets/img/anikina.png';
import efremovaPhoto from '../assets/img/efremova.jpg';
import petrikPhoto from '../assets/img/petrik.png';
import malishevPhoto from '../assets/img/malishev.jpg';
import apalcovPhoto from '../assets/img/apalcov.jpg';
import pihtinPhoto from '../assets/img/pihtin.jpg';
import shirabakinaPhoto from '../assets/img/shirabakina.jpeg';
import tomakovaPhoto from '../assets/img/tomakova.jpg';
import wolfengagenPhoto from '../assets/img/wolfengagen.jpg';
import orlovaPhoto from '../assets/img/orlova.jpg';
import brezgnevaPhoto from '../assets/img/brezgneva.png';

const Photo = styled.img`
  position: relative;
  margin-bottom: 5px;
`;

const Section = styled.section`
  padding-top: 30px;
  padding-bottom: 30px;
  position: relative;
  height: 100%;
  background-color: #fff;
`;

const SectionTitle = styled.h2`
  font-size: 26px;
  font-weight: 400;
  line-height: normal;
  color: #2b55bc;
  text-align: center;
  padding-top: 10px;
  @media (min-width: 992px) {
    margin-bottom: 50px;
  }
`;

const SubTitle = styled.h5`
  font-size: 24px;
  font-weight: normal;
  line-height: 1.83;
  text-align: center;
  color: #000;
  @media (min-width: 992px) {
    max-width: 785px;
    margin-left: auto;
    margin-right: auto;
    margin-bottom: 49px;
  }
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

const BoxTitle = styled.h4`
  font-size: 18px;
  font-weight: 300;
  line-height: normal;
  color: #000;
`;

const Text = styled.p`
  font-size: 16px;
  font-weight: normal;
  line-height: 1.58;
  color: #000;
  margin-bottom: 0;
  max-width: 350px;
`;

const WrapBox = styled.div`
  margin-bottom: 30px;
`;

const About = () => {
  const width = window.innerWidth;
  return (
    <Section id='about'>
      <div className='container'>
        <SectionTitle>Организационный комитет</SectionTitle>
        <div className='row'>
          <WrapBox className='col-lg-6'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={pihtinPhoto} alt='Пыхтин Алексей Иванович' />
              <BoxTitle>
                Пыхтин Алексей Иванович, к.т.н., доцент, директор департамента
                информационных технологий и нового набора, ЮЗГУ.
              </BoxTitle>
              <Text>Председатель организационного комитета конференции</Text>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-6'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo
                src={shirabakinaPhoto}
                alt='Ширабакина Тамара Александровна'
              />
              <BoxTitle>
                Ширабакина Тамара Александровна, профессор, декан факультета
                фундаментальной и прикладной информатики, ЮЗГУ.
              </BoxTitle>
              <Text>Заместитель председателя организационного комитета</Text>
            </Box>
          </WrapBox>
        </div>

        <SubTitle>Состав организационного комитета</SubTitle>
        <div className='row'>
          <WrapBox className='col-lg-4'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={malishevPhoto} alt='Малышев А.В.' />
              <BoxTitle>
                Малышев А.В., доцент, заведующий кафедрой программной инженерии,
                ЮЗГУ.
              </BoxTitle>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-4'>
            <Box data-aos={width >= 1400 ? 'fade-right' : 'fade-up'}>
              <Photo src={anikinaPhoto} alt='Аникина Е.И.' />
              <BoxTitle>
                Аникина Е.И., кандидат технических наук, доцент кафедры
                программной инженерии ЮЗГУ.
              </BoxTitle>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-4'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={petrikPhoto} alt='Петрик Е.А.' />
              <BoxTitle>
                Петрик Е.А., к.т.н., доцент кафедры программной инженерии ЮЗГУ.
              </BoxTitle>
            </Box>
          </WrapBox>
        </div>

        <SectionTitle>Программный комитет</SectionTitle>
        <div className='row'>
          <WrapBox className='col-lg-6'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={pihtinPhoto} alt='Пыхтин Алексей Иванович' />
              <BoxTitle>
                Пыхтин Алексей Иванович, к.т.н., доцент, директор департамента
                информационных технологий и нового набора, ЮЗГУ.
              </BoxTitle>
              <Text>Председатель программного комитета конференции</Text>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-6'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={tomakovaPhoto} alt='Томакова Римма Александровна' />
              <BoxTitle>
                Томакова Римма Александровна, доктор технических наук,
                профессор, ЮЗГУ.
              </BoxTitle>
              <Text>Заместитель председателя программного комитета</Text>
            </Box>
          </WrapBox>
        </div>

        <SubTitle>Состав программного комитета</SubTitle>
        <div className='row'>
          <WrapBox className='col-lg-4'>
            <Box data-aos='fade-up'>
              <Photo src={wolfengagenPhoto} alt='Вольфенгаген В.Э.' />
              <BoxTitle>
                Вольфенгаген В.Э., доктор технических наук, профессор, кафедра
                кибернетики ФГАОУВПО «Национальный исследовательский ядерный
                университет «МИФИ», г. Москва, Россия
              </BoxTitle>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-4'>
            <Box data-aos='fade-up'>
              <Photo src={orlovaPhoto} alt='Орлова Ю.А.' />
              <BoxTitle>
                Орлова Ю.А., доктор технических наук, доцент, зав. кафедры
                программного обеспечения автоматизированных систем
                Волгоградского технического университета, г. Волгоград, Россия
              </BoxTitle>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-4'>
            <Box data-aos='fade-up'>
              <Photo src={brezgnevaPhoto} alt='Брежнева А.Н.' />
              <BoxTitle>
                Брежнева А.Н., кандидат технических наук, доцент кафедры
                информатики ФГБОУ ВПО "РЭУ им. Плеханова", г. Москва, Россия
              </BoxTitle>
            </Box>
          </WrapBox>
        </div>

        <SectionTitle>Технический комитет</SectionTitle>
        <div className='row'>
          <WrapBox className='col-lg-6'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={malishevPhoto} alt='Малышев А.В.' />
              <BoxTitle>
                Малышев А.В., доцент, заведующий кафедрой программной инженерии,
                ЮЗГУ.
              </BoxTitle>
              <Text>Председатель программного комитета конференции</Text>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-6'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={anikinaPhoto} alt='Аникина Е.И.' />
              <BoxTitle>
                Аникина Е.И., кандидат технических наук, доцент кафедры
                программной инженерии ЮЗГУ.
              </BoxTitle>
              <Text>Заместитель председателя программного комитета</Text>
            </Box>
          </WrapBox>
        </div>

        <SubTitle>Состав технического комитета</SubTitle>
        <div className='row'>
          <WrapBox className='col-lg-6'>
            <Box data-aos={width >= 1400 ? 'fade-left' : 'fade-up'}>
              <Photo src={apalcovPhoto} alt='Апальков В.В.' />
              <BoxTitle>
                Апальков В.В., к.т.н., доцент кафедры программной инженерии
                ЮЗГУ.
              </BoxTitle>
            </Box>
          </WrapBox>

          <WrapBox className='col-lg-6'>
            <Box data-aos='fade-up'>
              <Photo src={efremovaPhoto} alt='Ефремова И.Н.' />
              <BoxTitle>
                Ефремова И.Н., к.т.н., доцент кафедры программной инженерии
                ЮЗГУ, технический секретарь.
              </BoxTitle>
            </Box>
          </WrapBox>
        </div>
      </div>
    </Section>
  );
};

export default About;
