import CanvasComponent from '../components/Canvas';
import { useNavigate } from 'react-router-dom';

function ExportDessin({ ws }) {
    const navigate = useNavigate();

    const handleExportSuccess = () => {
        navigate('/WaitingState');
    };

    return (
        <>
            <h1>Dessinez ce qu'il vous plaît!</h1>
            <CanvasComponent ws={ws} onExportSuccess={handleExportSuccess} />
        </>
    );
}

export default ExportDessin;
