import {useLocation, useNavigate} from "react-router-dom";
import styles from "./navbar.module.css"
import logo from "./logo.svg"
import symbol from "./symbol.svg"

const NavBar = () => {
    const navigate = useNavigate();
    const location = useLocation();

    function handleNavigation(page) {

        navigate(`/${page}`)
    }

    return (
        <div className={styles.navbar}>
            <div className={styles.logoAndButtons}>
                <div className={styles.navbarLogo}><img src={logo} alt="logo"></img></div>
                <div>
                    <button
                        className={location.pathname === "/" ? styles.activeButton + " " + styles.frontPageButton : styles.frontPageButton}
                        onClick={() => handleNavigation('')}>AVALEHT
                    </button>
                    <button
                        className={location.pathname === "/events" ? styles.activeButton + " " + styles.addEventButton : styles.addEventButton}
                        onClick={() => handleNavigation('events')}>ÜRITUSE LISAMINE
                    </button>
                </div>
            </div>
            <div className={styles.navbarSymbol}><img src={symbol} alt="symbol"></img></div>
        </div>
    )
}

export default NavBar;