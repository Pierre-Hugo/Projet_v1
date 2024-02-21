import React, { useEffect } from 'react';
import '../styles/WaitingRoom.css';
import image1 from '../img/prop1.png';
import image2 from '../img/prop2.png';
import image3 from '../img/prop3.png';
import image4 from '../img/prop4.png';
import image5 from '../img/prop5.png';
import image6 from '../img/prop6.png';
import image7 from '../img/prop7.png';
import image8 from '../img/prop8.png';
import image9 from '../img/prop9.png';
import image10 from '../img/prop10.png';
import image11 from '../img/prop11.png';
import image12 from '../img/prop12.png';
import image13 from '../img/prop13.png';
import image14 from '../img/prop14.png';
import image15 from '../img/prop15.png';
import image16 from '../img/prop16.png';
import image17 from '../img/prop17.png';
import image18 from '../img/prop18.png';

function WaitingRoom() {
  useEffect(() => {
    const generateImages = () => {
      const numberOfImages = 5;
      const container = document.querySelector('.container');

      const images = [
        image1, image2, image3, image4, image5, image6, image7, image8, image9,
        image10, image11, image12, image13, image14, image15, image16, image17, image18
      ];

      for (let i = 0; i < numberOfImages; i++) {
        const image = document.createElement('img');
        const randomImageIndex = Math.floor(Math.random() * images.length);
        const randomSize = Math.random() * 50 + 30;
        const randomX = Math.random() * (window.innerWidth - randomSize);
        const randomAnimationDuration = Math.random() * 5 + 2;

        image.setAttribute('src', images[randomImageIndex]);
        image.classList.add('rain-image');
        image.style.width = `${randomSize}px`;
        image.style.position = 'absolute';
        image.style.left = `${randomX}px`;
        image.style.top = `-${randomSize}px`; 
        image.style.animationDuration = `${randomAnimationDuration}s`;
        image.style.transition = `top ${randomAnimationDuration}s linear`; 

        container.appendChild(image);

        setTimeout(() => {
          image.style.top = `${window.innerHeight}px`;
        }, 0);
      }
      container.addEventListener('transitionend', handleTransitionEnd);
    };
    generateImages();
    const interval = setInterval(generateImages, 2500);
    return () => clearInterval(interval);
  }, []);

  
  const handleTransitionEnd = (event) => {
    const container = document.querySelector('.container');
    const image = event.target;

    if (container.contains(image)) {
      container.removeChild(image);
    }
  };

  return (
    <div className="container">
      <h1 className="title">En attente des joueurs<span className="dots"></span></h1>
      <div className="loader"></div>
    </div>
  );
}

export default WaitingRoom;