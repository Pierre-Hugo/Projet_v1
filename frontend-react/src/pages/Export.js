function Export() {

    return (
      <>
            <h1>Choissisez un média pour commencer la partie !</h1>
              <form>
                <label>
                    Mot:
                    <input type="text" value="" />
                </label>
                <br />
                <br />
                <label>
                    Dessin:
                    <input type="file" value="" />
                </label>
                <br />
                <br />
                <label>
                    Image:
                    <input type="file" value="" />
                </label>
                <br />
                <br />
                <label>
                    Vidéo:
                    <input type="file" value="" />
                </label>
                <br />
                <br />
                <button type="submit">Sousmettre</button>
            </form>
      </>
        );
  
  }
  
  export default Export;