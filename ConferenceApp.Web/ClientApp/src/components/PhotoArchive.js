import React from "react";
import styled from "styled-components";

import member1Photo from '../assets/img/member1.jpg';
import member2Photo from '../assets/img/member2.jpg';
import member3Photo from '../assets/img/member3.jpg';
import member4Photo from '../assets/img/member4.jpg';

const Section = styled.section`
  padding-top: 30px;
  padding-bottom: 30px;
  height: 100%;
  background-color: #fff;
`;

const SectionTitle = styled.h2`
  font-size: 26px;
  font-weight: 400;
  line-height: normal;
  color: #1d4dbb;
  text-align: center;
  margin-bottom: 70px;
`;

const Box = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  margin-bottom: 10px;
  padding-bottom: 10px;
  &:hover {
    transform: translateY(-5px) !important;
  }

  @media (min-width: 992px) {
    margin-bottom: 0;
  }
`;

const PhotoWrap = styled.div`
  position: relative;
  max-width: 800px;
  margin: 0 auto;
`;

const Photo = styled.img`
  width: 100%;
`;

const TextToPhoto = styled.div`
  position: absolute;
  bottom: 0;
  background: rgb(0, 0, 0);
  background: rgba(0, 0, 0, 0.5);
  color: #f1f1f1;
  width: 100%;
  padding: 20px;

  @media (max-width: 450px) {
    font-size: 12px;
  }
`;

const PhotoArchive = () => {
  const width = window.innerWidth;
  return (
    <Section id="photo-archive">
      <div className="container">
        <SectionTitle>Фотоархив</SectionTitle>
        <div className="row">
          <div className="col-lg-6">
            <Box data-aos={width >= 1400 ? "fade-right" : "fade-up"}>
              <PhotoWrap>
                <Photo src={member1Photo} alt="Фото 1 участников" />
                <TextToPhoto>
                  <p>Макашин В.А., Томаков М.В. СОВЕРШЕНСТВОВАНИЕ АВТОМАТИЗАЦИИ ПРОЦЕССА АНАЛИЗА И КЛАССИФИКАЦИИ ФЛЮОРОГРАММ ГРУДНОЙ КЛЕТКИ</p>
                </TextToPhoto>
              </PhotoWrap>
            </Box>
          </div>
          <div className="col-lg-6">
            <Box data-aos={width >= 1400 ? "fade-left" : "fade-up"}>
              <PhotoWrap>
                <Photo src={member2Photo} alt="Фото 2 участников" />
                <TextToPhoto>
                  <p>Гордеева В.В., Шамин К.В. ПРОГРАММА ОПТИМИЗАЦИИ РАЗМЕЩЕНИЯ ИСТОЧНИКОВ ОСВЕЩЕНИЯ НА СТРОИТЕЛЬНЫХ ПЛОЩАДКАХ</p>
                </TextToPhoto>
              </PhotoWrap>
            </Box>
          </div>
          <div className="col-lg-6">
            <Box data-aos={width >= 1400 ? "fade-left" : "fade-up"}>
              <PhotoWrap>
                <Photo src={member3Photo} alt="Фото 3 участников" />
                <TextToPhoto>
                  <p>Минаев Д.П., Алексеев В.А., Корсунский Н.А. МЕТОД ВЫДЕЛЕНИЯ ГРАНИЦ ОБЪЕКТОВ НА СЛОЖНОСТРУКТУРИРУЕМЫХ ИЗОБРАЖЕНИЯХ С ИСПОЛЬЗОВАНИЕМ МОРФОЛОГИЧЕСКИХ ОПЕРАТОРОВ</p>
                </TextToPhoto>
              </PhotoWrap>
            </Box>
          </div>
          <div className="col-lg-6">
            <Box data-aos={width >= 1400 ? "fade-left" : "fade-up"}>
              <PhotoWrap>
                <Photo src={member4Photo} alt="Фото 4 участников" />
                <TextToPhoto>
                  <p>Шоморова Д.И., Минаев Д.П. ПРОГРАММА АВТОМАТИЗАЦИИ СИСТЕМЫ УПРАВЛЕНИЯ ПОЖАРНОЙ БЕЗОПАСНОСТЬЮ ЗДАНИЙ НА ОСНОВЕ СИСТЕМЫ SCADA</p>
                </TextToPhoto>
              </PhotoWrap>
            </Box>
          </div>
        </div>
      </div>

    </Section>
  );
};

export default PhotoArchive;
