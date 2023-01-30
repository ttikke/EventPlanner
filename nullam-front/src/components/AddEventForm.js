import image from "./libled.jpg"
import styles from "./addeventform.module.css"
import axios from "axios"
import {useState} from "react";
import {useNavigate} from "react-router-dom";

function checkDateTimeFormat(string) {
    const regex = new RegExp("^([1-9]|([012][0-9])|(3[01]))\\.([0]{0,1}[1-9]|1[012])\\.\\d\\d\\d\\d\\s([0-1]?[0-9]|2?[0-3]):([0-5]\\d)$")
    return regex.test(string)
}

const AddEventForm = () => {
    const [isDateValid, setIsDateValid] = useState(true)
    const [isDetailsValid, setIsDetailsValid] = useState(true)

    const navigate = useNavigate();
    function handleNavigation(page) {
        navigate(`/${page}`)
    }

    const [name, setName] = useState("")
    const [date, setDate] = useState("")
    const [location, setLocation] = useState("")
    const [details, setDetails] = useState("")


    const ConvertTime = (formTime) => {
        const formattedDate = new Date()
        const list = formTime.split(" ")
        const date = list[0].split(".")
        const time = list[1].split(":")

        formattedDate.setFullYear(parseInt(date[2]), parseInt(date[1]), parseInt(date[0]))
        formattedDate.setHours(parseInt(time[0]))
        formattedDate.setMinutes(parseInt(time[1]))

        return formattedDate
    }

    const HandleSubmit = async (event) => {
        event.preventDefault()
        const convertedTime = ConvertTime(date)
        const formValues = {
            name: name,
            startTime: convertedTime,
            location: location,
            details: details,
            participants: [],
            corporations: []
        };
        const json = JSON.stringify(formValues);

        await axios.post("https://localhost:7158/api/v1/Event", json, {
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            console.log(response)
            handleNavigation("")
        }).catch(function (error) {
            console.log(error)
        });
    }

    return (
        <div className={styles.addEventForm}>
            <div className={styles.header}>
                <div className={styles.headerText}>Ürituse lisamine</div>
                <img src={image} alt="libled"></img>
            </div>
            <div className={styles.formContainer}>
                <form className={styles.formWithHeaderAndButtons} onSubmit={HandleSubmit}>
                    <div className={styles.formHeader}>Ürituse lisamine</div>
                    <div className={styles.formElements}>
                        <div className={styles.form}>
                            <div>Ürituse nimi:</div>
                            <input value={name} onChange={(e) => setName(e.target.value)}/>
                        </div>
                        <div className={styles.form}>
                            <div>Toimumisaeg:</div>
                            <input placeholder="pp.kk.aaaa hh:mm" value={date}
                                   pattern="^[0-9.: ]+$"
                                   onChange={(e) =>  {
                                       setIsDateValid(e.target.value.length === 0 ? true : (checkDateTimeFormat(e.target.value)))
                                       setDate((s) => e.target.validity.valid ? e.target.value : s)
                                   }}/>
                        </div>
                        {!isDateValid && <div className={styles.form}>
                            <div></div>
                            <div className={styles.error}>Kuupäev ja kellaaeg väär</div>
                        </div> }
                        <div className={styles.form}>
                            <div>Koht:</div>
                            <input value={location} onChange={(e) => setLocation(e.target.value)}/>
                        </div>
                        <div className={styles.form}>
                            <div>Lisainfo:</div>
                            <textarea value={details} onChange={(e) => {
                                setIsDetailsValid(e.target.value.length <= 1000)
                                if(isDetailsValid) {
                                    setDetails(e.target.value)
                                }
                            }}/>
                        </div>
                        {!isDetailsValid && <div>
                            <div></div>
                            <div className={styles.error}>Lisainfo peab olema maksimaalselt 1000 tähemärki</div>
                        </div>}
                    </div>
                    <div className={styles.buttons}>
                        <button className={styles.backButton} onClick={() => navigate(-1)}>Tagasi</button>
                        <button className={styles.addButton} type="submit">Lisa</button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default AddEventForm;