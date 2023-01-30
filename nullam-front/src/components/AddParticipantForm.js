import styles from "./addparticipantform.module.css"
import {useState} from "react";
import axios from "axios";
import {useLocation, useNavigate, useParams} from "react-router-dom";


function handlePaymentType(paymentType) {
    if (paymentType === "Cash") {
        return 0
    }
    return 1
}

function checkIdCode(string) {
    const regex = new RegExp("(^[1-6]{1}[0-9]{2}[0-1]{1}[0-9]{1}[0-2]{1}[0-9]{1}[0-9]{4}$)")
    return regex.test(string)
}

const AddParticipantForm = () => {
    const {id} = useParams();
    const {state} = useLocation()

    const participantId = state ? state.participant.id : null
    const [formFirstName, setFirstName] = useState(state ?  state.participant.firstName : '')
    const [formLastName, setLastName] = useState(state ?  state.participant.lastName : '')
    const [formIdCode, setIdCode] = useState(state ? state.participant.idNumber : '')
    const [formPaymentType, setPaymentType] = useState(state ? state.participant.paymentType : '0')
    const [formDetails, setDetails] = useState(state ? state.participant.details : "")

    const [idCodeValid, setIdCodeValid] = useState(true)
    const [isDetailsValid, setIsDetailsValid] = useState(true)

    const navigate = useNavigate();

    const HandleSubmit = async (event) => {
        event.preventDefault()
        const correctPaymentType = handlePaymentType(formPaymentType)

        const formValues = {
            firstName: formFirstName,
            lastName: formLastName,
            idNumber: formIdCode,
            paymentType: correctPaymentType,
            details: formDetails,
        };
        const json = JSON.stringify(formValues);
        console.log(json)


        if(participantId == null) {
            await axios.post("https://localhost:7158/api/v1/Event/" + id + "/participants", json, {
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
                console.log(response)
                window.location.reload()
            }).catch(function (error) {
                console.log(error)
            });
        } else {
            await axios.put("https://localhost:7158/api/v1/Event/" + id + "/participants/" + participantId, json, {
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
                console.log(response)
                navigate(-1)
            }).catch(function (error) {
                console.log(error)
            });
        }

    }

    return (
        <div>
            <form className={styles.formWithButtons} onSubmit={HandleSubmit}>
                <div className={styles.form}>
                    <div className={styles.formNames}>
                        <div>Eesnimi:</div>
                        <input value={formFirstName} onChange={(e) => setFirstName(e.target.value)}/>
                    </div>
                    <div className={styles.formNames}>
                        <div>Perenimi:</div>
                        <input value={formLastName} onChange={(e) => setLastName(e.target.value)}/>
                    </div>
                    <div className={styles.formNames}>
                        <div>Isikukood:</div>
                        <input value={formIdCode}
                               pattern="[0-9]*"
                               onChange={(e) => {
                                   setIdCodeValid(e.target.value.length === 0 ? true : (checkIdCode(e.target.value)))
                                   setIdCode((a) => e.target.validity.valid ? e.target.value : a)
                               }}/>
                    </div>
                    {!idCodeValid && <div className={styles.formNames}>
                        <div></div>
                        <div className={styles.error}>Isikukood väär</div>
                    </div>}
                    <div className={styles.formNames}>
                        <div>Maksmisviis:</div>
                        <select value={formPaymentType} onChange={(e) => setPaymentType(e.target.value)}>
                            <option value="Cash">Sularaha</option>
                            <option value="Transfer">Ülekanne</option>
                        </select>
                    </div>
                    <div className={styles.formNames}>
                        <div>Lisainfo:</div>
                        <textarea value={formDetails} onChange={(e) => {
                            setIsDetailsValid(e.target.value.length <= 1500)
                            if(isDetailsValid) {
                                setDetails(e.target.value)
                            }
                        }}/>
                    </div>
                    {!isDetailsValid && <div className={styles.formNames}>
                        <div></div>
                        <div className={styles.error}>Lisainfo peab olema maksimaalselt 1500 tähemärki</div>
                    </div>}
                </div>
                <div className={styles.buttons}>
                    <button className={styles.backButton} type="button" onClick={() => navigate(-1)}>Tagasi</button>
                    <button className={styles.addButton} type="submit">Salvesta</button>
                </div>
            </form>
        </div>
    )
}

export default AddParticipantForm;