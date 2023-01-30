import styles from "./footer.module.css"

const Footer = () => {
    return (
        <div className={styles.footer}>
            <div>
                <h2 className={styles.footerHeader}>Curabitur</h2>
                <div>Emauris</div>
                <div>Kfringilla</div>
                <div>Oin magna sem</div>
                <div>Kelementum</div>
            </div>
            <div>
                <h2 className={styles.footerHeader}>Fusce</h2>
                <div>Econsectur</div>
                <div>Ksollicitudin</div>
                <div>Omvulputate</div>
                <div>Nunc fringilla tellu</div>
            </div>
            <div className={styles.contact}>
                <div>
                    <h2 className={styles.footerHeader}>Kontakt</h2>
                    <h3>Peakontor: Tallinnas</h3>
                    <div>Väike-Ameerika 1, 11415</div>
                    <div>Telefon: 6054450</div>
                    <div>Faks: 605 3186</div>
                </div>
                <div>
                    <h2 className={styles.footerHeader}>&nbsp;</h2>
                    <h3>Harukontor: Võrus</h3>
                    <div>Oja tn 7(külastusaadress)</div>
                    <div>Telefon: 605 3330</div>
                    <div>Faks: 605 3155</div>
                </div>
            </div>
        </div>
    )
}

export default Footer;