import AddCorporationForm from "../AddCorporationForm.js";
import {render} from "@testing-library/react";

test('renders without crashing', () => {
    render(<AddCorporationForm/>)
})