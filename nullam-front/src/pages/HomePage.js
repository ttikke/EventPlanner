import NavBar from "../components/NavBar";
import Footer from "../components/footer"
import Intro from "../components/introduction"
import Events from "../components/Events"

import styles from "./homepage.module.css"
import layout from "./layout.module.css"

import axios from "axios";
import {useEffect, useState} from "react";
import {formatDate} from "../helpers/date";


const getEvents = async () => {
    const response = await axios.get("https://localhost:7158/api/v1/Event")

    const pastEvents = []
    const upcomingEvents = []
    const currentTime = new Date().toJSON();

    const allEvents = response.data.events
    for (const event of allEvents) {
        if (event.startTime < currentTime) {
            event.startTime = formatDate(event.startTime)
            pastEvents.push(event)
        } else {
            event.startTime = formatDate(event.startTime)
            upcomingEvents.push(event)
        }
    }
    return {past: pastEvents, upcoming: upcomingEvents}
}

const HomePage = () => {
    const [events, setEvents] = useState({past: [], upcoming: []})
    useEffect(() => {
        getEvents().then(e => setEvents(e))
    }, [])

    return (
        <div className={layout.layout}>
            <div className={layout.innerLayout}>
                <NavBar/>
                <Intro/>
                <div className={styles.events}>
                    <div className={styles.element}><Events header={"Tulevased üritused"} isUpcoming={true}
                                                            events={events.upcoming}/></div>
                    <div className={styles.element}><Events header={"Toimunud üritused"} events={events.past}/></div>
                </div>
                <Footer/>
            </div>
        </div>
    )
}

export default HomePage;