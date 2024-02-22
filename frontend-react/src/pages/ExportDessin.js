import CanvasComponent from '../components/Canvas';
import { useNavigate } from 'react-router-dom';

function ExportDessin({ ws }) {
    const navigate = useNavigate();

    const handleExportSuccess = () => {
        navigate('/WaitingState');
    };

    return (
        <div className="container"> 
            <div className="canvas-container"> 
                <CanvasComponent ws={ws} onExportSuccess={handleExportSuccess} />
            </div>
        </div>
    );
}

export default ExportDessin;
