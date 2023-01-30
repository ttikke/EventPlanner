import NavBar from "../components/NavBar";
import Footer from "../components/footer"
import ParticipantDetails from "../components/ParticipantDetails"

import layout from "./layout.module.css"



const ParticipantDetailsPage = () => {
    return (
        <div className={layout.layout}>
            <div className={layout.innerLayout}>
                <NavBar/>
                <ParticipantDetails/>
                <Footer/>
            </div>
        </div>
    )
}
export default ParticipantDetailsPage;