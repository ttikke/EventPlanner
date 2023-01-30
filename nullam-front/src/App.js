import {BrowserRouter, Routes, Route} from "react-router-dom";

import HomePage from "./pages/HomePage";
import AddEventPage from "./pages/AddEventPage";
import EventDetailsPage from "./pages/EventDetailsPage";
import ParticipantDetailsPage from "./pages/ParticipantDetailsPage";

const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<HomePage/>}/>
                <Route path="/events" element={<AddEventPage/>}/>
                <Route path="/events/:id" element={<EventDetailsPage/>}/>
                <Route path="/events/:id/participant/:participantId" element={<ParticipantDetailsPage/>}/>
            </Routes>
        </BrowserRouter>
    )
}

export default App;
