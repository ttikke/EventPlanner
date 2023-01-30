
const formatDate = (date) => {
    const options = {day: '2-digit', month: '2-digit', year: 'numeric'};
    const dateParts = new Date(date).toLocaleDateString("en-us",options).split("/")
    const correctDate = [dateParts[1], dateParts[0], dateParts[2]]
    return correctDate.join(".")
}
export {formatDate}
