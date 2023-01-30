import styles from "./events.module.css"
import Remove from "./remove.svg"
import {useNavigate} from "react-router-dom";
import axios from "axios";


const Events = ({header, isUpcoming, events}) => {
    const navigate = useNavigate();

    function handleNavigation(page) {
        navigate(`/${page}`)
    }

    const deleteEvent = async (event, id) => {
        event.preventDefault()
        await axios.delete("https://localhost:7158/api/v1/Event/" + id)
            .then(function (response) {
                console.log(response)
                window.location.reload()
                }
            )
    }

    return (
        <div className={styles.events}>
            <div className={styles.header}>{header}</div>
            <div className={styles.listWithAddButton}>
                <div className={styles.list}>
                    {events && events.map((e, index) => (
                        <div className={styles.row} key={index}>
                            <div className={styles.eventName}>{index + 1}. {e.name}</div>
                            <div className={styles.otherElements}>
                                <div className={styles.date}>{e.startTime}</div>
                                <div className={styles.actions}>
                                    <div onClick={() => handleNavigation('events/' + e.id)}><b>OSAVÕTJAD</b></div>
                                    {isUpcoming &&
                                        <div><img onClick={event => deleteEvent(event, e.id)} width="12px" height="12px"
                                                  src={Remove} alt="Remove"></img>
                                        </div>}
                                </div>
                            </div>
                        </div>))}
                </div>
                {isUpcoming &&
                    <div className={styles.addEvent} onClick={() => handleNavigation('events')}><b>LISA ÜRITUS</b>
                    </div>}
            </div>
        </div>
    )
}

export default Events;