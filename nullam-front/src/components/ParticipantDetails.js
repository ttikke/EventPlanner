import styles from "./participantdetails.module.css"

import AddParticipantForm from "../components/AddParticipantForm"
import image from "./libled.jpg"
import {useLocation} from "react-router-dom";
import AddCorporationForm from "./AddCorporationForm";

const ParticipantDetails = () => {

    const {state} = useLocation()
    const {isParticipant, participant} = state

    return (
        <div className={styles.participantDetails}>
            <div className={styles.header}>
                <div className={styles.headerText}>Osavõtja info</div>
                <img src={image} alt="libled"></img>
            </div>
            <div className={styles.participantDetailsContainer}>
                <div className={styles.participantContent}>
                    <div className={styles.innerHeader}>Osavõtjad</div>
                    {isParticipant && <div><AddParticipantForm/></div>}
                    {!isParticipant && <div><AddCorporationForm/></div>}
                </div>
            </div>

        </div>
    )
}

export default ParticipantDetails;