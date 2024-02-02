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

    const getTouchPos = (e) => {
      const rect = canvas.getBoundingClientRect();
      return {
        x: e.touches[0].clientX - rect.left,
        y: e.touches[0].clientY - rect.top,
      };
    };

    canvas.addEventListener('touchstart', (e) => {
      drawing = true;
      ctx.beginPath();
      ctx.strokeStyle = this.state.currentColor;
      const pos = getTouchPos(e);
      ctx.moveTo(pos.x, pos.y);
      e.preventDefault();
    });

    canvas.addEventListener('touchmove', (e) => {
      if (!drawing) return;
      const pos = getTouchPos(e);
      ctx.lineTo(pos.x, pos.y);
      ctx.stroke();
      e.preventDefault();
    });

    canvas.addEventListener('touchend', () => {
      drawing = false;
      ctx.closePath();
      this.path.push(ctx.getImageData(0, 0, canvas.width, canvas.height));
    });

    this.clearButtonRef.current.addEventListener('click', () => {
      if (this.path.length > 0) {
        this.clearCanvas();
        this.path.pop();
        this.path.forEach((pathData) => ctx.putImageData(pathData, 0, 0));
      }
    });

    this.clearAllRef.current.addEventListener('click', () => {
      this.clearCanvas();
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

    const sizes = [
      { id: 'small', size: 1 },
      { id: 'medium', size: 3 },
      { id: 'large', size: 7 },
      { id: 'fill', size: 17 },
    ];

    sizes.forEach((sizeData) => {
      const button = document.getElementById(sizeData.id);
      button.addEventListener('click', () => {
        ctx.lineWidth = sizeData.size;
      });
    });

    ctx.lineWidth = 3;
  }

  clearCanvas = () => {
    const canvas = this.canvasRef.current;
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
  };

  exportCanvasAsJPEG = () => {
    const canvas = this.canvasRef.current;

    // Obtenez les données de l'image en format base64
    const imageDataURL = canvas.toDataURL('image/jpeg', 0.9);

    // Affichez l'URL des données dans la console
    console.log('Image Data URL:', imageDataURL);
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
        <input type="radio" id="black" name="color" defaultChecked />
        <label htmlFor="black">Noir</label>
        <br />
        <input type="radio" id="red" name="color" />
        <label htmlFor="red">Rouge</label>
        <br />
        <input type="radio" id="blue" name="color" />
        <label htmlFor="blue">Bleu</label>
        <br />
        <input type="radio" id="green" name="color" />
        <label htmlFor="green">Vert</label>
        <br />
        <input type="radio" id="white" name="color" />
        <label htmlFor="white">Blanc</label>
        <br />
        <br />
        <input type="radio" id="small" name="size" />
        <label htmlFor="small">Pointe Fine</label>
        <br />
        <input type="radio" id="medium" name="size" defaultChecked />
        <label htmlFor="medium">Classique</label>
        <br />
        <input type="radio" id="large" name="size" />
        <label htmlFor="large">Gras</label>
        <br />
        <input type="radio" id="fill" name="size" />
        <label htmlFor="fill">Remplissage</label>
        <br />
        <br />
        <button ref={this.clearButtonRef}>Retour en arrière</button>
        <button ref={this.clearAllRef}>Tout effacer</button>
        <button onClick={this.exportCanvasAsJPEG}>Exporter en JPEG</button>
      </div>
    );
  }
}

export default CanvasComponent;
