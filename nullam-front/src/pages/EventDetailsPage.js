import NavBar from "../components/NavBar";
import Footer from "../components/footer"
import EventDetails from "../components/EventDetails"

import layout from "./layout.module.css"

const EventDetailsPage = () => {
  return (
    <div className={layout.layout}>
      <div className={layout.innerLayout}>
        <NavBar/>
        <EventDetails/>
        <Footer/>
      </div>
    </div>
  )
}
export default EventDetailsPage;