import React, { Component } from 'react';

class CanvasComponent extends Component {
  constructor(props) {
    super(props);
    this.canvasRef = React.createRef();
    this.clearButtonRef = React.createRef();
    this.clearAllRef = React.createRef();
    this.path = [];
    this.currentColor = '#000000';
  }

  componentDidMount() {
    const canvas = this.canvasRef.current;
    const ctx = canvas.getContext('2d');

    let drawing = false;

    canvas.addEventListener('mousedown', (e) => {
      drawing = true;
      ctx.beginPath();
      ctx.strokeStyle = this.currentColor;
      ctx.moveTo(e.clientX - canvas.getBoundingClientRect().left, e.clientY - canvas.getBoundingClientRect().top);
    });

    canvas.addEventListener('mousemove', (e) => {
      if (!drawing) return;
      ctx.lineTo(e.clientX - canvas.getBoundingClientRect().left, e.clientY - canvas.getBoundingClientRect().top);
      ctx.stroke();
    });

    canvas.addEventListener('mouseup', () => {
      drawing = false;
      ctx.closePath();
      this.path.push(ctx.getImageData(0, 0, canvas.width, canvas.height));
    });

    canvas.addEventListener('mouseout', () => {
      drawing = false;
    });

    this.clearButtonRef.current.addEventListener('click', () => {
      if (this.path.length > 0) {
        this.path.pop();
        this.clearCanvas();
        this.path.forEach((pathData) => ctx.putImageData(pathData, 0, 0));
      }
    });

    this.clearAllRef.current.addEventListener('click', () => {
      ctx.clearRect(0, 0, canvas.width, canvas.height);
    });

    const colors = [
      { id: 'black', color: '#000000' },
      { id: 'red', color: '#ff0000' },
      { id: 'blue', color: '#0000ff' },
      { id: 'green', color: '#00ff00' },
      { id: 'white', color: '#ffffff' },
    ];

    colors.forEach((colorData) => {
      const button = document.getElementById(colorData.id);
      button.addEventListener('click', () => {
        this.currentColor = colorData.color; // Mettez à jour la couleur actuelle
        ctx.strokeStyle = this.currentColor;
      });
    });

    ctx.lineWidth = 3;
  }

  clearCanvas = () => {
    const canvas = this.canvasRef.current;
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
  };

  render() {
    return (
      <div>
        <canvas
          ref={this.canvasRef}
          width={400}
          height={400}
          style={{ border: '2px solid black' }}
        ></canvas>
        <br/>
        <input type="radio" id="black" name="color"></input>
        <label for="color">Noir</label>
        <br/>
        <input type="radio" id="red" name="color"></input>
        <label for="color">Rouge</label>
        <br/>
        <input type="radio" id="blue" name="color"></input>
        <label for="color">Bleu</label>
        <br/>
        <input type="radio" id="green" name="color"></input>
        <label for="color">Vert</label>
        <br/>
        <input type="radio" id="white" name="color"></input>
        <label for="color">Blanc</label>
        <br/>
        <button ref={this.clearButtonRef}>Retour en arrière</button>
        <button ref={this.clearAllRef}>Tout effacer</button>
      </div>
    );
  }
}

export default CanvasComponent;