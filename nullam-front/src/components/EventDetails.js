import styles from "./eventdetails.module.css"
import {useNavigate} from "react-router-dom";
import image from "./libled.jpg"
import AddParticipantForm from "../components/AddParticipantForm"
import {useParams} from "react-router-dom";
import axios from "axios";
import {useEffect, useState} from "react";
import {formatDate} from "../helpers/date";
import AddCorporationForm from "./AddCorporationForm";


const getEvent = async (id) => {
    const response = await axios.get("https://localhost:7158/api/v1/Event/" + id)
    const event = response.data
    event.startTime = formatDate(event.startTime)
    console.log(event)
    return event
}

const EventDetails = () => {
    const {id} = useParams();
    const navigate = useNavigate();

    const deleteEventParticipant = async (event, participantId) => {
        event.preventDefault()
        await axios.delete("https://localhost:7158/api/v1/Event/" + id + "/participants/" + participantId)
            .then(function (response) {
                    console.log(response)
                    window.location.reload()
                }
            )
    }

    const deleteEventCorporation = async (event, corporationId) => {
        event.preventDefault()
        await axios.delete("https://localhost:7158/api/v1/Event/" + id + "/corporations/" + corporationId)
            .then(function (response) {
                    console.log(response)
                    window.location.reload()
                }
            )
    }


    const [status, setStatus] = useState(1)

    const radioHandler = (status) => {
        setStatus(status)

    }

    const [event, setEvent] = useState({corporations: [], participants: []})

    useEffect(() => {
        getEvent(id).then(e => setEvent(e))
    }, [id])


    const participantsAndCorporations = event.participants.concat(event.corporations)

    return (
        <div className={styles.eventDetails}>
            <div className={styles.header}>
                <div className={styles.headerText}>Osavõtjad</div>
                <img src={image} alt="libled"></img>
            </div>

            <div className={styles.eventDetailsContainer}>
                <div className={styles.eventDetailsContent}>
                    <div className={styles.innerHeader}>Osavõtjad</div>
                    <div className={styles.eventDetailsInfo}>
                        <div className={styles.detailsName}>
                            <div>Ürituse nimi:</div>
                            <div>Toimumisaeg:</div>
                            <div>Koht:</div>
                            <div>Lisainfo:</div>
                            <div>Osavõtjad:</div>
                        </div>
                        <div className={styles.detailsInfo}>
                            <div>{event.name}</div>
                            <div>{event.startTime}</div>
                            <div>{event.location}</div>
                            <div>{event.details}</div>
                            <div className={styles.list}>
                                {participantsAndCorporations && participantsAndCorporations.map((e, index) => (
                                    <div className={styles.row} key={index}>
                                        <div className={styles.participantName}>{index + 1}. {e.name ? e.name : e.firstName + " " + e.lastName}</div>
                                        <div className={styles.otherElements}>
                                            <div>{e.idNumber ? e.idNumber : e.registrationCode}</div>
                                            <div className={styles.actions}>
                                                <div onClick={
                                                    () => navigate('/events/' + id + '/participant/' + e.id, {
                                                        state: {
                                                            isParticipant: !!e.firstName,
                                                            participant: e
                                                        }
                                                    })}>
                                                    <b>VAATA</b></div>
                                                <div onClick={
                                                    e.firstName ?
                                                        (event) => deleteEventParticipant(event, e.id) :
                                                        (event) => deleteEventCorporation(event, e.id)}>
                                                    <b>KUSTUTA</b></div>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                    <div className={styles.innerHeader}>Osavõtjate Lisamine</div>
                    <form className={styles.radios}>
                        <div><input name="participantType" type="radio" onClick={(e) => radioHandler(1)}
                                    checked={status === 1}></input> Eraisik
                        </div>
                        <div><input name="participantType" type="radio" onClick={(e) => radioHandler(2)}
                                    checked={status === 2}></input> Ettevõte
                        </div>
                    </form>
                    {status === 1 && <AddParticipantForm/>}
                    {status === 2 && <AddCorporationForm/>}
                </div>
            </div>
        </div>
    )

}

export default EventDetails;