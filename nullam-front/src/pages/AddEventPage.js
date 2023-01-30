import NavBar from "../components/NavBar";
import Footer from "../components/footer"
import AddEventForm from "../components/AddEventForm"

import styles from "./layout.module.css"

const AddEventPage = () => {
  return (
    <div className={styles.layout}>
      <div className={styles.innerLayout}>
        <NavBar/>
        <AddEventForm/>
        <Footer/>
      </div>
    </div>
  )
}
export default AddEventPage;