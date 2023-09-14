import { Link } from 'react-router-dom'

function Room() {

    return (
      <>
        <h1>Entrez le numéro de la room</h1>
        <form>
          <label>
            PIN:
            <input type="number" value=""/>
          </label>
          <Link to="/export">Connexion</Link>
        </form>
      </>
        );
  
  }
  
  export default Room;