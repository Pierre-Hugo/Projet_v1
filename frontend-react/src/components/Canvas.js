import React, { Component } from 'react';

class CanvasComponent extends Component {
  constructor(props) {
    super(props);
    this.canvasRef = React.createRef();
    this.clearButtonRef = React.createRef();
    this.clearAllRef = React.createRef();
    this.path = [];
    this.state = {
      currentColor: '#000000',
    };
  }

  componentDidMount() {
    const canvas = this.canvasRef.current;
    const ctx = canvas.getContext('2d');

    let drawing = false;

    canvas.addEventListener('mousedown', (e) => {
      drawing = true;
      ctx.beginPath();
      ctx.strokeStyle = this.state.currentColor;
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

    // Ajout d'un événement click pour dessiner un point fixe
    canvas.addEventListener('click', (e) => {
      ctx.fillStyle = this.state.currentColor;
      ctx.beginPath();
      ctx.arc(e.clientX - canvas.getBoundingClientRect().left, e.clientY - canvas.getBoundingClientRect().top, 2, 0, Math.PI * 2);
      ctx.fill();
      this.path.push(ctx.getImageData(0, 0, canvas.width, canvas.height));
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
      this.path = [];
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
        this.setState({ currentColor: colorData.color });
        ctx.strokeStyle = colorData.color;
        ctx.fillStyle = colorData.color;
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
        <br />
        <input
          type="radio"
          id="black"
          name="color"
          defaultChecked
        />
        <label htmlFor="black">Noir</label>
        <br />
        <input
          type="radio"
          id="red"
          name="color"
        />
        <label htmlFor="red">Rouge</label>
        <br />
        <input
          type="radio"
          id="blue"
          name="color"
        />
        <label htmlFor="blue">Bleu</label>
        <br />
        <input
          type="radio"
          id="green"
          name="color"
        />
        <label htmlFor="green">Vert</label>
        <br />
        <input
          type="radio"
          id="white"
          name="color"
        />
        <label htmlFor="white">Blanc</label>
        <br />
        <button ref={this.clearButtonRef}>Retour en arrière</button>
        <button ref={this.clearAllRef}>Tout effacer</button>
      </div>
    );
  }
}

export default CanvasComponent;