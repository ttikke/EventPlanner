import styles from "./addparticipantform.module.css"
import {useLocation, useNavigate, useParams} from "react-router-dom";
import {useState} from "react";
import axios from "axios";


const AddCorporationForm = () => {
    const {id} = useParams();
    const {state} = useLocation()

    const participantId = state ? state.participant.id : null
    const [formName, setName] = useState(state ? state.participant.name : '')
    const [formRegNr, setRegNr] = useState(state ? state.participant.registrationCode : '')
    const [formAmount, setAmount] = useState(state ? state.participant.numberOfParticipants : '')
    const [formPaymentType, setPaymentType] = useState(state ? state.participant.paymentType : '0')
    const [formDetails, setDetails] = useState(state ? state.participant.details : "")

    const [isDetailsValid, setIsDetailsValid] = useState(true)

    const navigate = useNavigate();

    function handlePaymentType(paymentType) {
        if (paymentType === "Cash") {
            return 0
        }
        return 1
    }

    const isEnabled = () => {
        return !(formName.length < 1 && formRegNr.length < 1 && formAmount.length < 1 && formDetails.length > 5001);

    }

    const HandleSubmit = async (event) => {
        event.preventDefault()
        const correctPaymentType = handlePaymentType(formPaymentType)

        const formValues = {
            name: formName,
            registrationCode: formRegNr,
            numberOfParticipants: formAmount,
            paymentType: correctPaymentType,
            details: formDetails,
        };
        const json = JSON.stringify(formValues);

        if (participantId == null) {
            await axios.post("https://localhost:7158/api/v1/Event/" + id + "/corporations", json, {
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
            await axios.put("https://localhost:7158/api/v1/Event/" + id + "/corporations/" + participantId, json, {
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
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
                        <div>Ettevõtte nimi:</div>
                        <input value={formName} onChange={(e) => setName(e.target.value)}/>
                    </div>
                    <div className={styles.formNames}>
                        <div>Registrikood:</div>
                        <input value={formRegNr} onChange={(e) => setRegNr(e.target.value)}/>
                    </div>
                    <div className={styles.formNames}>
                        <div>Osalejate arv:</div>
                        <input value={formAmount} pattern="[0-9]*"
                               onChange={(e) => setAmount((a) => e.target.validity.valid ? e.target.value : a)}/>
                    </div>
                    <div className={styles.formNames}>
                        <div>Maksmisviis:</div>
                        <select value={formPaymentType} onChange={(e) => setPaymentType(e.target.value)}>
                            <option value="0">Sularaha</option>
                            <option value="1">Ülekanne</option>
                        </select>
                    </div>
                    <div className={styles.formNames}>
                        <div>Lisainfo:</div>
                        <textarea value={formDetails} onChange={(e) => {
                            setIsDetailsValid(e.target.value.length <= 5000)
                            if (isDetailsValid) {
                                setDetails(e.target.value)
                            }
                        }}/>
                    </div>
                    {!isDetailsValid && <div>
                        <div></div>
                        <div className={styles.error}>Lisainfo peab olema maksimaalselt 5000 tähemärki</div>
                    </div>}
                </div>
                <div className={styles.buttons}>
                    <button className={styles.backButton} type="button" onClick={() => navigate(-1)}>Tagasi</button>
                    <button className={styles.addButton} disabled={!isEnabled()} type="submit">Salvesta</button>
                </div>
            </form>
        </div>
    )
}

export default AddCorporationForm;