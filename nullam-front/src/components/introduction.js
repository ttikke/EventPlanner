import styles from "./intro.module.css"
import image from "./pilt.jpg"


const Intro = () => {
    return (
        <div className={styles.introduction}>
            <div>
                <p className={styles.text}>Sed nec elit vestibulum, <b>tincidunt orci</b> et, sagittis ex.
                    Vestibulum rutrum <b>neque suscipit</b> ante mattis maximus.
                    Nulla non sapien <b>viverra, lobortis lorem non</b>, accumsan metus.
                </p>
            </div>
            <img src={image} alt="pilt"></img>
        </div>
    )
}

export default Intro;